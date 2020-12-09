# Goal Systems - Inventory Management API

## Índice

## Requisitos

1. Añadir elemento al inventario
2. Sacar un elemento del inventario
3. Notificar que un elemento se ha sacado del inventario
4. Notificar cuando un elemento caduca
 
[Volver](#índice)

---

## Requerimientos

Instalar .Net Core 3.1.x
Instalar las herramientas de EntityFrameworkCore

```shell
dotnet tool install --global dotnet-ef
```

Actualizar las herramientas de EntityFrameworkCore

```shell
dotnet tool update --global dotnet-ef
```

## Aplicar las migraciones de BBDD

> Nota: Todos los comando debe ejecutarse en el directorio `GoalSytemsAPI`, no en el raíz.

Listado de migraciones

```shell
dotnet ef migrations list
```

Aplicar todas las migraciones
```shell
dotnet ef database update
```

## Instalación de dependencias

> Nota: Todos los comando debe ejecutarse en el directorio `GoalSytemsAPI`, no en el raíz.

```shell
dotnet restore
```

[Volver](#índice)

---

## Ejecución

> Nota: Todos los comando debe ejecutarse en el directorio `GoalSytemsAPI`, no en el raíz.

```shell
dotnet run
```
 
[Volver](#índice)

---

## Swagger

Para relizar pruebas de la API, es necesario acceder a [Swagger UI](https://localhost:5001/swagger). La API está securizada mediante __JWT__ (_JSON Web Token_), por lo que será necesario generar un token para realizar las pruebas.
 
[Volver](#índice)

---

## Generar Token JWT

Para generar un token __JSON Web Token__, es necesario consumir el servicio mediante el siguiente comando _*nix_, o con aplicaciones como [Postman](https://www.postman.com/):

```shell
curl --request POST \
  --url https://vbermudez.eu.auth0.com/oauth/token \
  --header 'content-type: application/json' \
  --data '{"client_id":"qwyl6ClAbbpEqMXgB5mEZjFPMUyBD5xr","client_secret":"AdXooSgO3FWJ_Yo5e9OWMldF5kfyDTcquVYZRkadHCHe03qu6rPwSw9nbGHkd1z3","audience":"https://goalsystems.inventory.api/","grant_type":"client_credentials"}'
```

La respuesta generada, debería ser similar al siguiente _JSON_:

```json
{"access_token":"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik1CVnR2WngyRkZ0ZjJoRlNrclFfayJ9.eyJpc3MiOiJodHRwczovL3ZiZXJtdWRlei5ldS5hdXRoMC5jb20vIiwic3ViIjoicXd5bDZDbEFiYnBFcU1YZ0I1bUVaakZQTVV5QkQ1eHJAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vZ29hbHN5c3RlbXMuaW52ZW50b3J5LmFwaS8iLCJpYXQiOjE2MDc1Mzk4MjUsImV4cCI6MTYwNzYyNjIyNSwiYXpwIjoicXd5bDZDbEFiYnBFcU1YZ0I1bUVaakZQTVV5QkQ1eHIiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.QA5QmiQK7IXrD-Mqqiz59DPLo9MKiIAFmxBueqkdkE89Df2IG-ZUGVnFa1-CU0QqZnM10LEKccXvpY5tqKw2lKhDebuNRJgD4JR1ULb0QsfrxV0sgxi0rLDeH7G69SX_wIQqkwmpyandKJcK_Pa_wmgJdhOZaw6zqjpcXKB_NuO0RurBtGuvBjDRRSpDINcB7VYzM9Khi8UQCzHNqJXpeZn2nvpQ3QPvmwF69FiWN8VCLUoLe9ciDTwmLpS7X4QHA5eUXTUiD0rc3O7pZCwtdsigm-pXiWKbldrmWdFtG4vPZhgPSf4aNRJmxtC1K6D49p7kVuerzX0LrB6Oqyw8Dg","expires_in":86400,"token_type":"Bearer"}
```

Para realizar pruebas con la API, será necesario copiar el valor de `access_token` en el cuadro pertinente del interfaz __Swagger__, tal como sigue:

```text
Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik1CVnR2WngyRkZ0ZjJoRlNrclFfayJ9.eyJpc3MiOiJodHRwczovL3ZiZXJtdWRlei5ldS5hdXRoMC5jb20vIiwic3ViIjoicXd5bDZDbEFiYnBFcU1YZ0I1bUVaakZQTVV5QkQ1eHJAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vZ29hbHN5c3RlbXMuaW52ZW50b3J5LmFwaS8iLCJpYXQiOjE2MDc1Mzk4MjUsImV4cCI6MTYwNzYyNjIyNSwiYXpwIjoicXd5bDZDbEFiYnBFcU1YZ0I1bUVaakZQTVV5QkQ1eHIiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.QA5QmiQK7IXrD-Mqqiz59DPLo9MKiIAFmxBueqkdkE89Df2IG-ZUGVnFa1-CU0QqZnM10LEKccXvpY5tqKw2lKhDebuNRJgD4JR1ULb0QsfrxV0sgxi0rLDeH7G69SX_wIQqkwmpyandKJcK_Pa_wmgJdhOZaw6zqjpcXKB_NuO0RurBtGuvBjDRRSpDINcB7VYzM9Khi8UQCzHNqJXpeZn2nvpQ3QPvmwF69FiWN8VCLUoLe9ciDTwmLpS7X4QHA5eUXTUiD0rc3O7pZCwtdsigm-pXiWKbldrmWdFtG4vPZhgPSf4aNRJmxtC1K6D49p7kVuerzX0LrB6Oqyw8Dg
```

> Nota: Es importante encabezar el token con la palabra `Bearer`, seguida de un espacio, y luego el _JWT_: `Bearer token`

[Volver](#índice)

---

