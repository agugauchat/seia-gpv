# seia-gauchat-pellegrinet-volken
## Comentarios útiles:

A continuación, enumeramos una serie de comentarios que les podrían llegar a ser útiles al momento de corregir la entrega:

  - Dentro de la carpeta DB_Scripts, se encuentra el archivo DB_Structure.sql ([link](https://github.com/npelle/seia-gpv/blob/master/DB_Scripts/DB_Structure.sql)) para restaurar la base de datos que corresponde a la aplicación (hay que modificar el connectionString en el archivo Web.Config).
  - Tenemos configurado un brigde entre el repo y una aplicación en AppHarbor, por lo que podrían realizar pruebas en ese entorno ([Aplicación de test](http://devsoftheweb-test.apphb.com)).
  
**Nota:** Por cuestiones que todavía no resolvimos, en el link del mail de confimación, está añadiendo un puerto a la url de la app, eliminando eso, se puede confirmar el usuario sin problemas.
  - Url en el mail de confirmación: http://devsoftheweb-test.apphb.com:16971/Acco...
  - Url corregida: http://devsoftheweb-test.apphb.com/Acco...
