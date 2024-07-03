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

## Progress
```bash



Account 
- Regis =>  done
- Login => done
- Logout =>  done
- Profile =>  done
User
- Cart => done
- Checkout => done
- Get User orders => order details =>  done

Product
- Searching =>  done
- Filter =>  done


----------------
Admin
- Product CRUD => done

- Account 
--- Account Admin =>  done | Add accout => done
--- Account User =>  done  | Edit account => done
	
- Orders (list => detail =>update(status)) =>  done

- Others
--- ShippingFee Edit => done
--- Color CRUD => done

- Profile | show + update => done
- ChangePass by email => done

- DashBoard => done
--------
Authorization Done
 ----ADDING DATA ORDER PEDING....
```
