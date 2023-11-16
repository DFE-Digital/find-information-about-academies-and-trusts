locals {
  resource_prefix = "${local.environment}${local.project_name}"
}

resource "azurerm_storage_account" "ci-test-reports" {
  count = local.enable_ci_report_storage_container ? 1 : 0

  name                          = "${replace(local.resource_prefix, "-", "")}reports"
  resource_group_name           = module.azure_container_apps_hosting.azurerm_resource_group_default.name
  location                      = module.azure_container_apps_hosting.azurerm_resource_group_default.location
  account_tier                  = "Standard"
  account_replication_type      = "LRS"
  min_tls_version               = "TLS1_2"
  enable_https_traffic_only     = true
  public_network_access_enabled = true

  tags = local.tags
}

resource "azurerm_storage_container" "ci-test-reports" {
  count = local.enable_ci_report_storage_container ? 1 : 0

  name                  = "${local.resource_prefix}-reports"
  storage_account_name  = azurerm_storage_account.ci-test-reports[0].name
  container_access_type = "private"
}

resource "azurerm_monitor_diagnostic_setting" "ci-test-reports" {
  count = local.enable_ci_report_storage_container ? 1 : 0

  name                           = "${local.resource_prefix}-reports-diag"
  target_resource_id             = azurerm_storage_account.ci-test-reports[0].id
  log_analytics_workspace_id     = module.azure_container_apps_hosting.azurerm_log_analytics_workspace_container_app.id
  log_analytics_destination_type = "Dedicated"
  eventhub_name                  = local.enable_event_hub ? module.azure_container_apps_hosting.azurerm_eventhub_container_app.name : null

  metric {
    category = "Transaction"
  }
}

resource "azurerm_storage_management_policy" "ci-test-reports" {
  count = local.enable_ci_report_storage_container ? 1 : 0

  storage_account_id = azurerm_storage_account.ci-test-reports[0].id

  rule {
    name    = "ci-report-cool-off"
    enabled = true

    filters {
      blob_types = ["blockBlob"]
    }

    actions {
      base_blob {
        tier_to_cool_after_days_since_creation_greater_than = 7
        delete_after_days_since_creation_greater_than       = 14
      }

      snapshot {
        delete_after_days_since_creation_greater_than = 14
      }
    }
  }
}

data "azurerm_storage_account_blob_container_sas" "ci-test-reports" {
  count = local.enable_ci_report_storage_container ? 1 : 0

  connection_string = azurerm_storage_account.ci-test-reports[0].primary_connection_string
  container_name    = azurerm_storage_container.ci-test-reports[0].name
  https_only        = true

  start  = formatdate("YYYY-MM-DD'T'hh:mm:ssZ", timestamp())
  expiry = formatdate("YYYY-MM-DD'T'hh:mm:ssZ", timeadd(timestamp(), "+4380h")) # +6 months

  permissions {
    read   = true
    add    = true
    create = true
    write  = true
    delete = true
    list   = true
  }
}

output "ci-test-reports-storage-sas-url" {
  description = "A SAS tokenised URL for accessing the CI Reports in the Blob Storage Container"
  value       = local.enable_ci_report_storage_container ? nonsensitive("${azurerm_storage_account.ci-test-reports[0].primary_blob_endpoint}${azurerm_storage_container.ci-test-reports[0].name}${data.azurerm_storage_account_blob_container_sas.ci-test-reports[0].sas}") : null
}
