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

https://drive.google.com/file/d/1jiGiZCoV8hqiarTEcA--a-fzOpqV3hi0/view?usp=drive_link

