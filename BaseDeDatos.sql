USE master;
GO

-- 1. Evitar errores si la base ya existe
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'InventarioIT')
BEGIN
    CREATE DATABASE InventarioIT;
END
GO

USE InventarioIT;
GO

-- 2. LIMPIEZA TOTAL (Borramos en orden inverso para no romper relaciones)
IF OBJECT_ID('dbo.Mantenimientos', 'U') IS NOT NULL DROP TABLE dbo.Mantenimientos;
IF OBJECT_ID('dbo.Asignaciones', 'U') IS NOT NULL DROP TABLE dbo.Asignaciones;
IF OBJECT_ID('dbo.Hardware', 'U') IS NOT NULL DROP TABLE dbo.Hardware;
IF OBJECT_ID('dbo.Modelos', 'U') IS NOT NULL DROP TABLE dbo.Modelos;
IF OBJECT_ID('dbo.TiposHardware', 'U') IS NOT NULL DROP TABLE dbo.TiposHardware;
IF OBJECT_ID('dbo.Marcas', 'U') IS NOT NULL DROP TABLE dbo.Marcas;
IF OBJECT_ID('dbo.Proveedores', 'U') IS NOT NULL DROP TABLE dbo.Proveedores;
IF OBJECT_ID('dbo.Empleados', 'U') IS NOT NULL DROP TABLE dbo.Empleados;
IF OBJECT_ID('dbo.Ubicaciones', 'U') IS NOT NULL DROP TABLE dbo.Ubicaciones;
IF OBJECT_ID('dbo.Sedes', 'U') IS NOT NULL DROP TABLE dbo.Sedes;
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL DROP TABLE dbo.Usuarios;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;

-- 3. CREACIÓN DE TABLAS

CREATE TABLE Roles (
    RolID INT PRIMARY KEY IDENTITY(1,1),
    NombreRol VARCHAR(50) NOT NULL
);

CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) UNIQUE NOT NULL,
    Contrasena VARCHAR(50) NOT NULL,
    RolID INT FOREIGN KEY REFERENCES Roles(RolID)
);

CREATE TABLE Marcas (
    MarcaID INT PRIMARY KEY IDENTITY(1,1),
    NombreMarca VARCHAR(100) NOT NULL
);

CREATE TABLE TiposHardware (
    TipoID INT PRIMARY KEY IDENTITY(1,1),
    NombreTipo VARCHAR(100) NOT NULL
);

CREATE TABLE Modelos (
    ModeloID INT PRIMARY KEY IDENTITY(1,1),
    NombreModelo VARCHAR(100) NOT NULL,
    MarcaID INT FOREIGN KEY REFERENCES Marcas(MarcaID),
    TipoID INT FOREIGN KEY REFERENCES TiposHardware(TipoID)
);

CREATE TABLE Sedes (
    SedeID INT PRIMARY KEY IDENTITY(1,1),
    NombreSede VARCHAR(100) NOT NULL
);

CREATE TABLE Ubicaciones (
    UbicacionID INT PRIMARY KEY IDENTITY(1,1),
    NombreUbicacion VARCHAR(100) NOT NULL,
    SedeID INT FOREIGN KEY REFERENCES Sedes(SedeID)
);

CREATE TABLE Empleados (
    EmpleadoID INT PRIMARY KEY IDENTITY(1,1),
    NombreCompleto VARCHAR(100) NOT NULL,
    NumeroEmpleado VARCHAR(50) UNIQUE
);

CREATE TABLE Proveedores (
    ProveedorID INT PRIMARY KEY IDENTITY(1,1),
    NombreProveedor VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20)
);

CREATE TABLE Hardware (
    HardwareID INT PRIMARY KEY IDENTITY(1,1),
    NumeroSerie VARCHAR(100) UNIQUE NOT NULL,
    EtiquetaActivo VARCHAR(50),
    ModeloID INT FOREIGN KEY REFERENCES Modelos(ModeloID),
    FechaCompra DATE,
    Estado VARCHAR(50) DEFAULT 'Disponible'
);

CREATE TABLE Asignaciones (
    AsignacionID INT PRIMARY KEY IDENTITY(1,1),
    HardwareID INT FOREIGN KEY REFERENCES Hardware(HardwareID),
    EmpleadoID INT FOREIGN KEY REFERENCES Empleados(EmpleadoID),
    FechaAsignacion DATE DEFAULT GETDATE(),
    FechaDevolucion DATE NULL
);

CREATE TABLE Mantenimientos (
    MantenimientoID INT PRIMARY KEY IDENTITY(1,1),
    HardwareID INT FOREIGN KEY REFERENCES Hardware(HardwareID),
    ProveedorID INT FOREIGN KEY REFERENCES Proveedores(ProveedorID),
    DescripcionProblema VARCHAR(255),
    FechaEntrada DATE,
    FechaSalida DATE,
    Costo DECIMAL(10,2)
);

-- 4. DATOS INICIALES (PARA QUE EL LOGIN FUNCIONE)

INSERT INTO Roles (NombreRol) VALUES ('Administrador'), ('Trabajador');
INSERT INTO Usuarios (NombreUsuario, Contrasena, RolID) VALUES ('admin', 'admin123', 1);
INSERT INTO Usuarios (NombreUsuario, Contrasena, RolID) VALUES ('lalo', '1234', 2);

INSERT INTO Marcas (NombreMarca) VALUES ('Dell'), ('HP'), ('Lenovo'), ('Apple');
INSERT INTO TiposHardware (NombreTipo) VALUES ('Laptop'), ('PC Escritorio'), ('Monitor'), ('Impresora');
INSERT INTO Sedes (NombreSede) VALUES ('Edificio Central'), ('Planta Norte');
INSERT INTO Ubicaciones (NombreUbicacion, SedeID) VALUES ('Recepción', 1), ('Almacén IT', 2);
INSERT INTO Proveedores (NombreProveedor, Telefono) VALUES ('Soporte Oficial', '555-0000');

-- Modelo de prueba
INSERT INTO Modelos (NombreModelo, MarcaID, TipoID) VALUES ('Latitude 5420', 1, 1);

PRINT '✅ Base de datos restaurada con éxito.';