locals {
  resource_prefix = "${local.environment}${local.project_name}"
}

resource "azurerm_storage_account" "ci-test-reports" {
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
  name                  = "${local.resource_prefix}-reports"
  storage_account_name  = azurerm_storage_account.ci-test-reports.name
  container_access_type = "blob"
}

resource "azurerm_monitor_diagnostic_setting" "ci-test-reports" {
  name                           = "${local.resource_prefix}-reports-diag"
  target_resource_id             = azurerm_storage_account.ci-test-reports.id
  log_analytics_workspace_id     = module.azure_container_apps_hosting.azurerm_log_analytics_workspace_container_app.id
  log_analytics_destination_type = "Dedicated"
  eventhub_name                  = local.enable_event_hub ? module.azure_container_apps_hosting.azurerm_eventhub_container_app.name : null

  metric {
    category = "Transaction"

    retention_policy {
      enabled = false
    }
  }
}
