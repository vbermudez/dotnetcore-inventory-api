# Goal Systems - Inventory Management API

## Índice

## Requisitos

1. Añadir elemento al inventario
2. Sacar un elemento del inventario
3. Notificar que un elemento se ha sacado del inventario
4. Notificar cuando un elemento caduca
 
[Volver](#índice)

---

## Instalación de dependencias
 
[Volver](#índice)

---

## Ejecución
 
[Volver](#índice)

---

## Swagger

 
[Volver](#índice)

---

## Generar Token JWT

```shell
curl --request POST \
  --url https://vbermudez.eu.auth0.com/oauth/token \
  --header 'content-type: application/json' \
  --data '{"client_id":"qwyl6ClAbbpEqMXgB5mEZjFPMUyBD5xr","client_secret":"AdXooSgO3FWJ_Yo5e9OWMldF5kfyDTcquVYZRkadHCHe03qu6rPwSw9nbGHkd1z3","audience":"https://goalsystems.inventory.api/","grant_type":"client_credentials"}'
```

```json
{"access_token":"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik1CVnR2WngyRkZ0ZjJoRlNrclFfayJ9.eyJpc3MiOiJodHRwczovL3ZiZXJtdWRlei5ldS5hdXRoMC5jb20vIiwic3ViIjoicXd5bDZDbEFiYnBFcU1YZ0I1bUVaakZQTVV5QkQ1eHJAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vZ29hbHN5c3RlbXMuaW52ZW50b3J5LmFwaS8iLCJpYXQiOjE2MDc1Mzk4MjUsImV4cCI6MTYwNzYyNjIyNSwiYXpwIjoicXd5bDZDbEFiYnBFcU1YZ0I1bUVaakZQTVV5QkQ1eHIiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.QA5QmiQK7IXrD-Mqqiz59DPLo9MKiIAFmxBueqkdkE89Df2IG-ZUGVnFa1-CU0QqZnM10LEKccXvpY5tqKw2lKhDebuNRJgD4JR1ULb0QsfrxV0sgxi0rLDeH7G69SX_wIQqkwmpyandKJcK_Pa_wmgJdhOZaw6zqjpcXKB_NuO0RurBtGuvBjDRRSpDINcB7VYzM9Khi8UQCzHNqJXpeZn2nvpQ3QPvmwF69FiWN8VCLUoLe9ciDTwmLpS7X4QHA5eUXTUiD0rc3O7pZCwtdsigm-pXiWKbldrmWdFtG4vPZhgPSf4aNRJmxtC1K6D49p7kVuerzX0LrB6Oqyw8Dg","expires_in":86400,"token_type":"Bearer"}
```
 
[Volver](#índice)

---

