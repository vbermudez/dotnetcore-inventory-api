# Goal Systems - Inventory Management API

Documento técnico de una API REST para la _Gestión de Inventario_, según requisitos proporcionados.

## Índice

## Requisitos

Se debe porveer de una solución basada en el entorno __.Net Framework__, concretamente desarrolla con __C#__. La solución debe publicar una _API REST_, ofreciendo las siguientes funciones:

- Añadir elemento al inventario
- Sacar un elemento del inventario
- Notificar que un elemento se ha sacado del inventario
- Notificar cuando un elemento caduca
 
[Volver](#índice)

---

## Documentación

Entre las diferentes versiones de __.Net Framework__, es preferible utilizar la versión __.Net Core 3.1__, por modernidad y portabilidad. 
Dado que la _API_ debe interactuar con un origen de datos, presumiblemente relacional, se ha optado por incorporar _EntityFramework Core_ a la solución, concretamente la específica para _SQLite_, por simplicidad y portabilidad.
Además, dado que la _API_ es susceptible de ser publicada en una red abierta, se ha implementado un sencillo mecanismo de autenticación estándar utilizando _JSON Web Token_. 
La aplicación no prevee la asignación de roles, por lo que no se han implementado medidas de autorización a nivel de recursos.
Pare gestionar las notificaciones requeridas, se ha escogido la mecánica de los _Server Sent Events_, ya que se trata de un canal de comunicación unidireccional del servidor al cliente.

A continuación se detallan tanto el modelo de entidades, como el diagrama de secuencia de la _API_.

### Modelo

```mermaid
classDiagram
class InventoryItem {
	+long Id
	+string Barcode
	+string Name
	+string Description
	+long Units
	+DateTime ExpiryDate
	+string Location
}
```

Con este modelo se pretende exponer la posibilidad de realizar operaciones __CRUD__ tanto por el identificador único (Id), como a través del código de barras (Barcode) o el nombre (Name) del elemento.
Además, se han añadido un campo para describir el elemento (Description), aunque no es un valor requerido, así como el número de unidades existentes (Units), la fecha de caducidad (ExpiryDate) y la ubicación del elemento (Location).
Éste último campo refleja la necesidad de conocer la ubicación del elemento, como por ejemplo: la ciudad, el almaciés, el pasillo, la estatería, etc. Para mantener la sencillez de la solución, es un campo de texto libre, no requerido.

### Diagrama de secuencia

A continucación se presenta el detalle del diagrama de secuencias de la _API_, relativa a los recursos publicados, así como a su interacción con la base de datos.

> Nota: Las notificaciones al cliente se realizan a travér de __SSE__ (_Server Sent Events_).

```mermaid
sequenceDiagram
participant Client as RESTClient
participant API as InventoryAPI
participant SQL as SQLite
	
Note over Client,API: Create Item
Client-xAPI: POST
API->+SQL: Insert(IventoryItem)
SQL-->-API: Id
alt Status: Ok
	API--xClient: IventoryItem
else Status: Error
	API--xClient: Error
end

Note over Client,API: Read Item
Client-xAPI: GET
API->+SQL: Select(Id,Barcode,Name)
SQL-->-API: Item
alt Status: Ok
	API--xClient: IventoryItem
else Status: Error
	API--xClient: Error
end

Note over Client,API: Update Item
Client-xAPI: PUT
API->+SQL: Update(IventoryItem)
SQL-->-API: Void
alt Status: Ok
	API--xClient: Status 200
else Status: Error
	API--xClient: Error
end

Note over Client,API: Delete Item
Client-xAPI: DELETE
API->+SQL: Delete(Id)
SQL-->-API: Void
alt Status: Ok
	API--xClient: IventoryItem
else Status: Error
	API--xClient: Error
end

Note over Client,API: Item Extracted Notifications
Client--xAPI: SSE Subscription
opt Item Extracted
	API--xClient: Extraction Event
end
opt Item Expired
	API--xClient: Expiry Event
end
```

[Volver](#índice)

---

## Requerimientos de software

- Instalar .Net Core 3.1.x
- Instalar las herramientas de EntityFrameworkCore

```shell
dotnet tool install --global dotnet-ef
```

- Actualizar las herramientas de EntityFrameworkCore

```shell
dotnet tool update --global dotnet-ef
```

## Aplicar las migraciones de BBDD

> Nota: Todos los comando debe ejecutarse en el directorio `GoalSytemsAPI`, no en el raíz.

- Listado de migraciones

```shell
dotnet ef migrations list
```

- Aplicar todas las migraciones

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

Para relizar pruebas de la API, es necesario acceder a [Swagger UI](https://localhost:5001/swagger). La API está securizada mediante __JWT__ (_JSON Web Token_), por lo que será necesario generar un token para poder realizar las pruebas.
 
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

