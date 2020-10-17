# Todo

 - [ ] implement new request method <br>
``` 
{
    "request":"GET /database/collection/document"
}
// or 
{
    "request":"SET /database/collection/document",
    "data": {
        // data
    }
}
```
 - [ ] implement query requests <br>
```
{
    "request":"GET /database/collection/$"
    "query":"get where $name = 'Daytimer'"
}
```