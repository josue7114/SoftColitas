Create database SoftColitas
go

use SoftColitas
go

create table [tbUsuario](
IDUsuario int primary key not null identity (1,1),
NombreCompleto varchar(100) not null,
Username varchar(15) not null,
Correo varchar(100) not null,
Cedula varchar(15) not null,
Pass varchar(20) not null)
go

create table [tbRescate](
IDRescate int primary key not null identity (1,1),
CodigoAnimal varchar(15) not null,
LugarRescate varchar(30) not null,
FechaRescate Date not null,
EspecieAnimal varchar(30) not null,
DescripcionAnimal varchar(150) not null,
NombreQuienReporta varchar(50) null,
IDUsuario int not null)
go

Create table [tbFotosAnimales](
IDFoto int not null identity (1,1),
IDRescate int not null,
Foto image not null,
NombreImg varchar(500)
primary key(IDFoto, IDRescate))
go

create table [tbAdoptante](
IDAdoptante int identity (1,1) not null,
Cedula varchar(15) not null,
NombreCompleto varchar(100) not null,
Telefono varchar(15) not null,
Correo varchar(50) null,
Direccion varchar(100) not null,
CodigoAnimal varchar(15) not null,
EstadoCalificacion varchar(12) not null,
IDUsuario int not null,
primary key(IDAdoptante, Cedula))
go

create table [tbAnimal](
CodigoAnimal varchar(15) primary key not null,
Imagen image not null,
Tamano varchar(8) not null,
EdadAprox varchar(5) null,
PesoAprox varchar(5) null,
Color varchar(30) not null,
Estado varchar(15) not null,
Descripcion varchar(150) not null,
Especie varchar(30) not null,
FechaIngreso Date not null,
IDusuario int not null)
go

create table [tbExpedienteAnimal](
IDExpediente int primary key not null identity(1,1),
Peso varchar(5) not null,
FechaAtencion Date not null,
ProcedimientosRealizados varchar(150) not null,
Resumen varchar(1500) not null,
Vacuna varchar(2) not null,
CodigoAnimal varchar(15) not null)
go

create table [tbDonacion](
IDDonacion int primary key not null identity (1,1),
Monto decimal(8,2) not null,
Fecha date not null,
Tipo varchar(15) not null,
CodigoAnimal varchar(15) null,
IDUsuario int not null)
go

create table [tbArchivo](
IDArchivo int primary key identity(1,1) not null,
nombre varchar(100) not null,
archivo image not null)
go

alter table [tbDonacion] add foreign key (IDUsuario) references [tbUsuario] (IDUsuario)
go
alter table [tbRescate] add foreign key (IDUsuario) references [tbUsuario] (IDUsuario)
go
alter table [tbAdoptante] add foreign key (IDUsuario) references [tbUsuario] (IDUsuario)
go
alter table [tbAnimal] add foreign key (IDUsuario) references [tbUsuario] (IDUsuario)
go

alter table [tbAdoptante] add foreign key (CodigoAnimal) references [tbAnimal] (CodigoAnimal)
go
alter table [tbExpedienteAnimal] add foreign key (CodigoAnimal) references [tbAnimal] (CodigoAnimal)
go
alter table [tbDonacion] add foreign key (CodigoAnimal) references [tbAnimal] (CodigoAnimal)
go

---------------------------------------------------procedimientos almacenados CRUD-----------------------------------------------------------
---------------------------------------------------USUARIOS-----------------------------------------------------------

CREATE PROCEDURE SP_AGREGAR_USUARIO(@NOMBRE VARCHAR(100), @USERNAME VARCHAR(15), @CORREO VARCHAR(100), @CEDULA VARCHAR(15), @PASS VARCHAR(20)) AS
BEGIN
	INSERT INTO tbUsuario VALUES (@NOMBRE, @USERNAME, @CORREO, @CEDULA, @PASS)
END
GO

CREATE PROCEDURE SP_MODIFICAR_USUARIO(@USERNAME VARCHAR(15), @PASS VARCHAR(20)) AS
BEGIN
	UPDATE tbUsuario SET Pass = @PASS WHERE Username = @USERNAME
END
GO

CREATE PROCEDURE SP_ELIMINAR_USUARIO(@USERNAME VARCHAR(15)) AS
BEGIN
	DELETE FROM tbUsuario WHERE Username = @USERNAME
END
GO

CREATE PROCEDURE SP_BUSCAR_USUARIO(@USERNAME VARCHAR(15)) AS
BEGIN
	SELECT IDUsuario, NombreCompleto, Username, Correo, Cedula,Pass FROM TBUsuario WHERE Username = @USERNAME
END
GO

CREATE PROCEDURE SP_CONSULTAR_USUARIOS AS
BEGIN
	SELECT IDUsuario, NombreCompleto, Username, Correo, Cedula FROM TBUsuario 
END
GO

CREATE PROCEDURE SP_BUSCAR_USUARIO_CED(@CED VARCHAR(15)) AS
BEGIN
	SELECT IDUsuario, NombreCompleto, Username, Correo, Cedula FROM TBUsuario WHERE Cedula = @CED
END
GO
---------------------------------------------------ADOPTANTES-----------------------------------------------------------
CREATE PROCEDURE SP_AGREGAR_ADOPTANTE(@CEDULA VARCHAR(15), @NOMBRE_COMPLETO VARCHAR(100), @TELEFONO VARCHAR(15), @CORREO VARCHAR(50),
@DIRECCION VARCHAR(100), @CODIGO_ANIMAL VARCHAR(15), @ESTADO VARCHAR(12), @IDUSUARIO INT) AS
BEGIN
	INSERT INTO tbAdoptante VALUES (@CEDULA, @NOMBRE_COMPLETO, @TELEFONO, @CORREO, @DIRECCION, @CODIGO_ANIMAL, @ESTADO, @IDUSUARIO)
	UPDATE tbAnimal SET Estado = 'Adoptado' WHERE CodigoAnimal = @CODIGO_ANIMAL
END
GO

CREATE PROCEDURE SP_MODIFICAR_ADOPTANTE(@CEDULA VARCHAR(15), @TELEFONO VARCHAR(15), @DIRECCION VARCHAR(100), @CORREO VARCHAR(50), @ESTADO VARCHAR(12)) AS
BEGIN
	UPDATE tbAdoptante SET Telefono = @TELEFONO, Direccion = @DIRECCION, Correo = @CORREO, EstadoCalificacion = @ESTADO WHERE Cedula = @CEDULA
END
GO

CREATE PROCEDURE SP_ELIMINAR_ADOPTANTE(@CEDULA VARCHAR(15)) AS
BEGIN
	DELETE FROM tbAdoptante WHERE Cedula= @CEDULA
END
GO

CREATE PROCEDURE SP_BUSCAR_ADOPTANTE(@CEDULA VARCHAR(15)) AS
BEGIN
	SELECT IDAdoptante, Cedula, NombreCompleto, Telefono, Correo ,Direccion, CodigoAnimal, EstadoCalificacion, IDUsuario FROM tbAdoptante WHERE Cedula= @CEDULA
END
GO

CREATE PROCEDURE SP_CONSULTAR_ADOPTANTES AS
BEGIN
	SELECT IDAdoptante, Cedula, NombreCompleto, Telefono, Correo ,Direccion, CodigoAnimal, EstadoCalificacion, IDUsuario FROM tbAdoptante
END
GO

CREATE PROCEDURE SP_CARGAR_ANIMALES AS
BEGIN
	SELECT CodigoAnimal, Imagen, Estado, Especie, PesoAprox FROM tbAnimal
END
GO


---------------------------------------------------ANIMALES-----------------------------------------------------------
CREATE PROCEDURE SP_AGREGAR_ANIMAL(@COD_ANIMAL VARCHAR(15), @IMAGEN IMAGE, @TAMANO VARCHAR(8), @EDAD_APROX VARCHAR(5),
@PESO_APROX VARCHAR(5), @COLOR VARCHAR(30), @ESTADO VARCHAR(15), @DESCRIPCION VARCHAR(150), @ESPECIE VARCHAR(30), @FECHA_INGRESO DATE, @IDUSUARIO INT) AS
BEGIN
	INSERT INTO tbAnimal VALUES (@COD_ANIMAL, @IMAGEN, @TAMANO, @EDAD_APROX, @PESO_APROX, @COLOR, @ESTADO, @DESCRIPCION, @ESPECIE, @FECHA_INGRESO, @IDUSUARIO)
END
GO

CREATE PROCEDURE SP_MODIFICAR_ANIMAL(@COD_ANIMAL VARCHAR(15), @TAMANO VARCHAR(8), @EDAD_APROX VARCHAR(5),
@PESO_APROX VARCHAR(5), @COLOR VARCHAR(30), @ESTADO VARCHAR(15), @DESCRIPCION VARCHAR(150)) AS
BEGIN
	UPDATE tbAnimal SET Tamano = @TAMANO, EdadAprox = @EDAD_APROX, PesoAprox = @PESO_APROX, Color = @COLOR, Estado = @ESTADO, Descripcion = @DESCRIPCION 
	WHERE CodigoAnimal= @COD_ANIMAL
END
GO

CREATE PROCEDURE SP_ELIMINAR_ANIMAL(@COD_ANIMAL VARCHAR(15)) AS
BEGIN
	DELETE FROM tbAnimal WHERE CodigoAnimal= @COD_ANIMAL
END
GO

CREATE PROCEDURE SP_BUSCAR_ANIMAL(@COD_ANIMAL VARCHAR(15)) AS
BEGIN
	SELECT CodigoAnimal, Imagen, Tamano, EdadAprox, PesoAprox ,Color, Estado, Descripcion, Especie, FechaIngreso, IDUsuario 
	FROM tbAnimal WHERE CodigoAnimal= @COD_ANIMAL
END
GO

CREATE PROCEDURE SP_CONSULTAR_ANIMAL AS
BEGIN
	SELECT CodigoAnimal, Imagen, Tamano, EdadAprox, PesoAprox ,Color, Estado, Descripcion, Especie, FechaIngreso FROM tbAnimal
END
GO

---------------------------------------------------RESCATES-----------------------------------------------------------
CREATE PROCEDURE SP_AGREGAR_RESCATE(@COD_ANIMAL VARCHAR(15), @LUGAR_RESCATE VARCHAR(30), @FECHA_RESCATE DATE, @ESPECIE VARCHAR(30), @DESCRIPCION VARCHAR(150),
@NOMBRE_REPORTE VARCHAR(50), @IDUSUARIO INT) AS
BEGIN
	INSERT INTO tbRescate VALUES(@COD_ANIMAL, @LUGAR_RESCATE, @FECHA_RESCATE, @ESPECIE, @DESCRIPCION, @NOMBRE_REPORTE, @IDUSUARIO)
END
GO

CREATE PROCEDURE SP_AGREGAR_FOTO_RESCATE(@IDRESCATE INT, @NOMBRE_IMG VARCHAR(500), @FOTO IMAGE) AS
BEGIN
	INSERT INTO tbFotosAnimales VALUES (@IDRESCATE, @FOTO, @NOMBRE_IMG)
END
GO

CREATE PROCEDURE SP_MODIFICAR_RESCATE(@COD_ANIMAL VARCHAR(15), @DESCRIPCION VARCHAR(150)) AS
BEGIN
	UPDATE tbRescate SET DescripcionAnimal = @DESCRIPCION WHERE CodigoAnimal = @COD_ANIMAL
END
GO

CREATE PROCEDURE SP_BUSCAR_RESCATE(@COD_ANIMAL VARCHAR(15)) AS
BEGIN
	SELECT IDRescate, CodigoAnimal, LugarRescate, FechaRescate, DescripcionAnimal, NombreQuienReporta, IDUsuario FROM tbRescate WHERE CodigoAnimal = @COD_ANIMAL
END
GO

CREATE PROCEDURE SP_CONTAR_RESCATES AS
	SELECT TOP(1) IDRescate FROM tbRescate ORDER BY IDRescate DESC	
	RETURN
GO

CREATE PROCEDURE SP_OBTENER_IMAGENES (@COD_ANIMAL VARCHAR(15))AS
BEGIN
	SELECT IDFoto, A.IDRescate, Foto, NombreImg FROM tbFotosAnimales A INNER JOIN tbRescate B ON A.IDRescate = B.IDRescate
                        WHERE B.CodigoAnimal = @COD_ANIMAL
END
GO

CREATE PROCEDURE SP_MODIFICAR_RESCATES (@COD_ANIMAL VARCHAR(15), @DESCRIPCION VARCHAR(150)) AS
BEGIN
	UPDATE tbRescate SET DescripcionAnimal = @DESCRIPCION WHERE CodigoAnimal = @COD_ANIMAL
END
GO

CREATE PROCEDURE SP_CONSULTAR_RESCATE (@COD_ANIMAL VARCHAR(15)) AS
BEGIN
	SELECT  IDRescate, CodigoAnimal, LugarRescate, FechaRescate, EspecieAnimal, DescripcionAnimal, NombreQuienReporta FROM tbRescate WHERE CodigoAnimal =@COD_ANIMAL
END
GO

CREATE PROCEDURE SP_OBTENER_IMAGEN_POR_NOMBRE (@NOMBRE VARCHAR(500)) AS
BEGIN
	SELECT IDFoto, IDRescate, Foto, NombreImg FROM tbFotosAnimales WHERE NombreImg=@NOMBRE	
END
GO

---------------------------------------------------DONACIONES-----------------------------------------------------------


CREATE PROCEDURE SP_DONACIONES_ID (@COD_ANIMAL VARCHAR(15)) AS
BEGIN
	SELECT IDDonacion, Monto, Fecha, CodigoAnimal,IDUsuario FROM tbDonacion WHERE @COD_ANIMAL=CodigoAnimal	
END
GO

CREATE PROCEDURE SP_AGREGAR_DONACION (@MONTO DECIMAL(8,2), @FECHA DATE, @COD_ANIMAL VARCHAR(15),@IDUSUARIO INT) AS
BEGIN
	INSERT INTO tbDonacion VALUES(@MONTO, @FECHA, @COD_ANIMAL,@IDUSUARIO)
END
GO

---------------------------------------------------ARCHIVOS-----------------------------------------------------------


CREATE PROCEDURE SP_MANT_ARCHIVO(@NOMBRE VARCHAR(100), @ARCHIVO IMAGE) AS
BEGIN
	INSERT INTO TBARCHIVO (NOMBRE,ARCHIVO) VALUES (@NOMBRE, @ARCHIVO)	
END
GO

CREATE PROCEDURE SP_CONSULTAR_ARCHIVOS AS
BEGIN
	SELECT nombre FROM tbArchivo	
END
GO

CREATE PROCEDURE SP_OBTENER_ARCHIVO(@NOMBRE VARCHAR(100)) AS
BEGIN
	SELECT IDArchivo, nombre, archivo FROM tbArchivo where nombre=@NOMBRE	
END
GO

---------------------------------------------------EXPEDIENTE MEDICO-----------------------------------------------------------

CREATE PROCEDURE SP_REGISTRAR_PROCEDIMIENTO(@PESO VARCHAR(5), @FECHA DATE, @PROCEDIMIENTO VARCHAR(150), @RESUMEN VARCHAR(1500), @VACUNA VARCHAR(2), @COD_ANIMAL VARCHAR(15)) AS
BEGIN
	INSERT INTO tbExpedienteAnimal (Peso, FechaAtencion, ProcedimientosRealizados, Resumen, Vacuna, CodigoAnimal) VALUES (@PESO, @FECHA, @PROCEDIMIENTO, @RESUMEN, @VACUNA, @COD_ANIMAL)
END
GO


---------------------------------------------------procedimientos almacenados-----------------------------------------------------------



CREATE PROCEDURE SP_LLENAR_GRID_POR_CED (@CED varchar(15)) AS
	SELECT Cedula, NombreCompleto as Nombre, Correo, Username from tbUsuario where Cedula=@CED
GO

CREATE PROCEDURE SP_LLENAR_GRID_USUARIOS AS
	SELECT Cedula, NombreCompleto as Nombre, Correo, Username from tbUsuario
GO

CREATE PROCEDURE SP_LLENAR_GRID_POR_COD (@COD varchar(15)) AS
	SELECT CodigoAnimal as Código,Tamano as Tamaño, EdadAprox as Edad, PesoAprox as Peso, Color, Estado, Descripcion, Especie, FechaIngreso as Ingreso from tbAnimal where CodigoAnimal= @COD
GO

CREATE PROCEDURE SP_LLENAR_GRID_POR_FECHA AS
	SELECT CodigoAnimal as Código,Tamano as Tamaño, EdadAprox as Edad, PesoAprox as Peso, Color, Estado, Descripcion, Especie, FechaIngreso as Ingreso from tbAnimal order by FechaIngreso DESC
GO

CREATE PROCEDURE SP_LLENAR_GRID_POR_ESPECIE (@ESPECIE VARCHAR(30)) AS
	SELECT CodigoAnimal as Código,Tamano as Tamaño, EdadAprox as Edad, PesoAprox as Peso, Color, Estado, Descripcion, Especie, FechaIngreso as Ingreso from tbAnimal where Especie= @ESPECIE
GO

CREATE PROCEDURE SP_LLENAR_GRID_GENERAL AS
	SELECT CodigoAnimal as Código,Tamano as Tamaño, EdadAprox as Edad, PesoAprox as Peso, Color, Estado, Descripcion, Especie, FechaIngreso as Ingreso from tbAnimal
GO






