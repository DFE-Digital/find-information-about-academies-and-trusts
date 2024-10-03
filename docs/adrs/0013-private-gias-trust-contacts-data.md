# 13. Feed Internal GIAS Data Directly into Academies DB for Trust Contacts  
**Date**: 2024-10-03  

## Status  
Accepted  

## Context  
Currently, the private GIAS (Get Information About Schools) data, which includes email addresses for internal trust contacts (such as CFOs and Chairs of Trustees), is manually keyed into KIM. However, KIM only has email fields for Accounting Officers and does not capture emails for other roles such as CFO and Chair of Trustees. This creates a problem for our system, FIAT, which requires emails for all three roles (Accounting Officer, CFO, and Chair of Trustees).

At present, the private GIAS data is received weekly in a spreadsheet. An individual manually inputs this data into KIM, from which it is pipelined into the Academies DB. FIAT then retrieves the data from Academies DB. However, due to the limitations of KIM, we are currently missing emails for CFOs and Chairs of Trustees in FIAT, creating gaps in our data.

To resolve this, we have decided to bypass KIM for this specific dataset. The weekly GIAS spreadsheet will be fed directly into the Academies DB, populating a new table called `TadTrustGovernance`. This ensures that all required email addresses (including those of CFOs and Chairs of Trustees) will be captured accurately. The `MstrTrustGovernance` table, which sources its data from KIM, will no longer be used for this purpose. The data engineers on the programme will take responsibility for managing this ingestion process.

This decision provides us with up-to-date and complete information but introduces an overhead for the data engineers who will now manage this process manually. Mid- to long-term, this process will need to be reviewed as KIM is decommissioned, and an alternative data propagation method is identified.

## Decision  
We will:

- Stop relying on KIM for capturing emails for CFOs and Chairs of Trustees.
- Feed the private GIAS data spreadsheet directly into the Academies DB to populate the `TadTrustGovernance` table.
- Discontinue the use of the `MstrTrustGovernance` table for email retrieval.
- Task the data engineers with managing this weekly ingestion process.

This decision is made to ensure that FIAT has access to complete and accurate contact information for the Accounting Officer, CFO, and Chair of Trustees. The current method via KIM is no longer sufficient, given its limitations in capturing emails for non-Accounting Officer roles.

## Consequences  
- We will have full access to the required email addresses, ensuring FIAT serves accurate contact data.
- The data engineers will have additional responsibilities in managing the weekly ingestion process, which introduces an overhead.
- This approach ensures immediate resolution of the missing data issue but may need to be revisited as KIM is decommissioned in the future. At that point, alternative methods for propagating the trust governance data will be explored.
