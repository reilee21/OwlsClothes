# Owls






## Installation

DB

```bash
add-migration init -context OwlStoreContext -outputdir Migrations/Base	

update-database -Context OwlStoreContext

add-migration addIdentity -context OwlsIdentityContext -outputdir Migrations/Identity

update-database -Context OwlsIdentityContext
```
    
## Data

https://drive.google.com/file/d/1rdcK_NCz4J_0_wTsLl2FSiB2UJaby3R5/view?usp=sharing

