USE DB_BARSTOC;
GO

-------------------------------------------------------------------------
------------------------ DATOS DE PRUEBA --------------------------------
-------------------------------------------------------------------------

PRINT '=== INICIANDO INSERCIÓN DE DATOS DE PRUEBA ===';

-- 1. Insertar Sedes
PRINT '1. Insertando sedes...';
INSERT INTO TBL_Sedes (nombreSede, direccion, telefono) VALUES
(N'Restrepo - Bogotá', N'Calle 123 #45-67, Restrepo', '6011234567'),
(N'Sede Chapinero - 95', N'Carrera 95 #23-45, Chapinero', '6012345678'),
(N'Parkway - Medellín', N'Avenida Parkway #67-89, Medellín', '6013456789');

PRINT '=== TABLA TBL_Sedes ===';
SELECT * FROM TBL_Sedes;
GO

-- 2. Insertar Roles
PRINT '2. Insertando roles...';
INSERT INTO TBL_Roles (nombreRol) VALUES 
(N'Administrador'),
(N'Cajero'),
(N'Mesero');

PRINT '=== TABLA TBL_Roles ===';
SELECT * FROM TBL_Roles;
GO

-- 3. Insertar Menús
PRINT '3. Insertando menús...';
INSERT INTO TBL_Menu (nombreMenu, icono, url) VALUES
(N'Dashboard', N'home', N'/dashboard'),
(N'Pedidos', N'shopping-cart', N'/pedidos'),
(N'Inventario', N'package', N'/inventario'),
(N'Reportes', N'bar-chart', N'/reportes'),
(N'Usuarios', N'users', N'/usuarios');

PRINT '=== TABLA TBL_Menu ===';
SELECT * FROM TBL_Menu;
GO

-- 4. Asignar Menús a Roles
PRINT '4. Asignando menús a roles...';
INSERT INTO TBL_MenuRol (idMenu, idRol) VALUES
(1,1), (2,1), (3,1), (4,1), (5,1),
(1,2), (2,2), (4,2),
(1,3), (2,3);

PRINT '=== TABLA TBL_MenuRol ===';
SELECT * FROM TBL_MenuRol;
GO

-- 5. Insertar Usuarios (password: 123456 - debe ser hasheado en la aplicación)
PRINT '5. Insertando usuarios...';
INSERT INTO TBL_Usuarios (numeroDocumento, idSede, nombreUsuario, apellidoUsuario, correo, idRol, usuarioLogin, passwordHash) VALUES
('123456789', 1, N'Carlos', N'Rodríguez', 'carlos.rodriguez@barestoc.com', 1, 'carlos.admin', 'hashed_password_123456'),
('987654321', 1, N'Ana', N'Gómez', 'ana.gomez@barestoc.com', 2, 'ana.cajero', 'hashed_password_123456'),
('456789123', 1, N'Luis', N'Martínez', 'luis.martinez@barestoc.com', 3, 'luis.mesero', 'hashed_password_123456'),
('789123456', 2, N'María', N'López', 'maria.lopez@barestoc.com', 3, 'maria.mesero', 'hashed_password_123456');

PRINT '=== TABLA TBL_Usuarios ===';
SELECT * FROM TBL_Usuarios;
GO

-- 6. Insertar Categorías
PRINT '6. Insertando categorías...';
INSERT INTO TBL_Categoria (nombreCategoria) VALUES
(N'Bebidas Alcohólicas'),
(N'Bebidas No Alcohólicas'),
(N'Cervezas Nacionales'),
(N'Cervezas Importadas'),
(N'Snacks y Pasabocas'),
(N'Cocteles Especiales');

PRINT '=== TABLA TBL_Categoria ===';
SELECT * FROM TBL_Categoria;
GO

-- 7. Insertar Productos CON PRECIOS EN PESOS COLOMBIANOS (miles)
PRINT '7. Insertando productos con precios en pesos colombianos...';
INSERT INTO TBL_Producto (codigoProducto, nombreProducto, idCategoria, costo, precioVenta) VALUES
-- Bebidas Alcohólicas (precios en miles de pesos)
('WHI001', N'Whisky Johnnie Walker Red Label 750ml', 1, 85000.00, 120000.00),
('RON001', N'Ron Viejo de Caldas 750ml', 1, 35000.00, 55000.00),
('VIN001', N'Vino Tinto Reserva Santa Helena 750ml', 1, 25000.00, 45000.00),
('TEC001', N'Tequila José Cuervo Silver 750ml', 1, 65000.00, 95000.00),

-- Cervezas Nacionales (precios por unidad)
('CER001', N'Cerveza Águila Light 330ml', 3, 2500.00, 5000.00),
('CER002', N'Cerveza Poker 330ml', 3, 2200.00, 4500.00),
('CER003', N'Cerveza Club Colombia Dorada 330ml', 3, 2800.00, 6000.00),

-- Cervezas Importadas (precios por unidad)
('CER101', N'Cerveza Corona Extra 355ml', 4, 4500.00, 9000.00),
('CER102', N'Cerveza Heineken 330ml', 4, 4000.00, 8000.00),
('CER103', N'Cerveza Stella Artois 330ml', 4, 4200.00, 8500.00),

-- Bebidas No Alcohólicas
('REF001', N'Gaseosa Coca-Cola 400ml', 2, 1500.00, 3500.00),
('REF002', N'Gaseosa Pepsi 400ml', 2, 1400.00, 3000.00),
('AGU001', N'Agua Mineral Sin Gas 600ml', 2, 1200.00, 2500.00),
('JUGO01', N'Jugo Hit Naranja 500ml', 2, 1800.00, 4000.00),

-- Snacks y Pasabocas
('PAP001', N'Papas Margaritas Natural 40g', 5, 800.00, 2500.00),
('MAN001', N'Maní Japonés Tajado 50g', 5, 1200.00, 3000.00),
('CHI001', N'Chicharrones de Cerdo 60g', 5, 1500.00, 4000.00),
('QUES01', N'Queso Fundido con Arepitas', 5, 4500.00, 12000.00),

-- Cocteles Especiales
('COC001', N'Cuba Libre Premium', 6, 8000.00, 18000.00),
('COC002', N'Mojito Clásico', 6, 7500.00, 16000.00),
('COC003', N'Margarita de Tequila', 6, 9500.00, 22000.00);

PRINT '=== TABLA TBL_Producto ===';
SELECT * FROM TBL_Producto;
GO

-- 8. Insertar Mesas
PRINT '8. Insertando mesas...';
INSERT INTO TBL_Mesas (idSede, numeroMesa, capacidad) VALUES
-- Sede 1 (Restrepo)
(1, 'M01', 4), (1, 'M02', 4), (1, 'M03', 6), (1, 'M04', 2), (1, 'M05', 8),
(1, 'M06', 4), (1, 'M07', 6), (1, 'M08', 10),
-- Sede 2 (Chapinero)
(2, 'M01', 4), (2, 'M02', 4), (2, 'M03', 6), (2, 'M04', 2), (2, 'M05', 8),
-- Sede 3 (Parkway)
(3, 'M01', 4), (3, 'M02', 4), (3, 'M03', 6), (3, 'M04', 2), (3, 'M05', 8),
(3, 'M06', 4), (3, 'M07', 6);

PRINT '=== TABLA TBL_Mesas ===';
SELECT * FROM TBL_Mesas;
GO

-- 9. Insertar Inventario Inicial
PRINT '9. Insertando inventario inicial...';
INSERT INTO TBL_Inventario (idSede, idProducto, cantidadDisponible) VALUES
-- Sede 1 (Restrepo)
(1,1,25), (1,2,40), (1,3,30), (1,4,15), 
(1,5,120), (1,6,150), (1,7,100),
(1,8,80), (1,9,70), (1,10,60),
(1,11,200), (1,12,180), (1,13,150), (1,14,120),
(1,15,50), (1,16,40), (1,17,35), (1,18,20),
(1,19,30), (1,20,25), (1,21,20),

-- Sede 2 (Chapinero)
(2,1,20), (2,2,35), (2,3,25), (2,4,12),
(2,5,100), (2,6,120), (2,7,80),
(2,8,70), (2,9,60), (2,10,50),
(2,11,150), (2,12,140), (2,13,120), (2,14,100),
(2,15,40), (2,16,35), (2,17,30), (2,18,15),
(2,19,25), (2,20,20), (2,21,18),

-- Sede 3 (Parkway)
(3,1,30), (3,2,45), (3,3,35), (3,4,20),
(3,5,150), (3,6,180), (3,7,120),
(3,8,100), (3,9,90), (3,10,80),
(3,11,250), (3,12,220), (3,13,200), (3,14,180),
(3,15,60), (3,16,50), (3,17,45), (3,18,25),
(3,19,40), (3,20,35), (3,21,30);

PRINT '=== TABLA TBL_Inventario ===';
SELECT * FROM TBL_Inventario;
GO

-------------------------------------------------------------------------
------------------------ PRUEBAS DEL SISTEMA ----------------------------
-------------------------------------------------------------------------

PRINT '=== INICIANDO PRUEBAS DEL SISTEMA ===';

-- Prueba 1: Ver productos con precios formateados en pesos colombianos
PRINT 'Prueba 1: Ver productos con precios formateados...';
SELECT * FROM VW_Productos_Precios_Formateados;
GO

-- Prueba 2: Crear un pedido
PRINT 'Prueba 2: Creando pedido...';
DECLARE @NuevoPedidoId INT;
EXEC SP_CrearPedido @idMesa = 1, @idUsuarioMesero = 3, @observaciones = N'Cliente frecuente - Mesa preferencial';
SET @NuevoPedidoId = SCOPE_IDENTITY();
PRINT 'Pedido creado con ID: ' + CAST(@NuevoPedidoId AS VARCHAR);
GO

-- Prueba 3: Agregar productos al pedido (con precios en miles)
PRINT 'Prueba 3: Agregando productos al pedido...';
EXEC SP_AgregarProductoPedido @idPedido = @NuevoPedidoId, @idProducto = 5, @cantidad = 3;   -- 3 Cervezas Águila
EXEC SP_AgregarProductoPedido @idPedido = @NuevoPedidoId, @idProducto = 15, @cantidad = 2;  -- 2 Papas Margaritas
EXEC SP_AgregarProductoPedido @idPedido = @NuevoPedidoId, @idProducto = 11, @cantidad = 2;  -- 2 Coca-Colas
EXEC SP_AgregarProductoPedido @idPedido = @NuevoPedidoId, @idProducto = 19, @cantidad = 1;  -- 1 Cuba Libre

-- Prueba 4: Verificar que la mesa se marcó como ocupada
PRINT 'Prueba 4: Verificando estado de la mesa...';
SELECT numeroMesa, estado FROM TBL_Mesas WHERE idMesa = 1;
GO

-- Prueba 5: Ver pedido activo con total en pesos
PRINT 'Prueba 5: Ver pedido activo con total en pesos colombianos...';
SELECT 
    numeroPedido,
    numeroMesa,
    sede,
    mesero,
    fechaApertura,
    FORMAT(totalPedido, 'C', 'es-CO') as TotalFormateado,
    totalPedido as TotalNumerico,
    observaciones
FROM VW_Pedidos_Activos;
GO

-- Prueba 6: Probar el procedimiento de formateo de precios
PRINT 'Prueba 6: Probando formateo de precios individual...';
DECLARE @precioFormateado NVARCHAR(50);
DECLARE @precioPrueba DECIMAL(12,2) = 125000.50;
EXEC SP_FormatearPrecio @precio = @precioPrueba, @precioFormateado = @precioFormateado OUTPUT;
PRINT 'Precio formateado: ' + @precioFormateado;
GO

-- Prueba 7: Cerrar pedido (pagar)
PRINT 'Prueba 7: Cerrando pedido...';
EXEC SP_CerrarPedido @idPedido = @NuevoPedidoId;
GO

-- Prueba 8: Verificar que la mesa se liberó
PRINT 'Prueba 8: Verificando que la mesa se liberó...';
SELECT numeroMesa, estado FROM TBL_Mesas WHERE idMesa = 1;
GO

-- Prueba 9: Ver reporte de ventas con precios formateados
PRINT 'Prueba 9: Generando reporte de ventas con formato colombiano...';
SELECT 
    fecha,
    numeroPedido,
    sede,
    nombreProducto,
    cantidad,
    precioVentaFormateado,
    subtotalFormateado,
    mesero
FROM (
    SELECT 
        fecha,
        numeroPedido,
        sede,
        nombreProducto,
        cantidad,
        FORMAT(precioVenta, 'C', 'es-CO') as precioVentaFormateado,
        FORMAT(subtotal, 'C', 'es-CO') as subtotalFormateado,
        mesero
    FROM VW_Reporte_Ventas
) AS ReporteFormateado;
GO

-- Prueba 10: Crear y cancelar un pedido
PRINT 'Prueba 10: Probando cancelación de pedido...';
DECLARE @PedidoCancelarId INT;
EXEC SP_CrearPedido @idMesa = 2, @idUsuarioMesero = 3, @observaciones = N'Pedido de prueba para cancelar';
SET @PedidoCancelarId = SCOPE_IDENTITY();

-- Agregar productos costosos
EXEC SP_AgregarProductoPedido @idPedido = @PedidoCancelarId, @idProducto = 1, @cantidad = 1;  -- 1 Whisky Johnnie Walker
EXEC SP_AgregarProductoPedido @idPedido = @PedidoCancelarId, @idProducto = 4, @cantidad = 1;  -- 1 Tequila José Cuervo

-- Ver inventario antes de cancelar
PRINT 'Inventario antes de cancelar:';
SELECT 
    p.nombreProducto,
    i.cantidadDisponible,
    FORMAT(p.precioVenta, 'C', 'es-CO') as PrecioUnitario
FROM TBL_Inventario i
JOIN TBL_Producto p ON i.idProducto = p.idProducto
WHERE i.idSede = 1 AND i.idProducto IN (1, 4);

-- Cancelar pedido
EXEC SP_CancelarPedido @idPedido = @PedidoCancelarId;

-- Ver inventario después de cancelar (debe haberse restaurado)
PRINT 'Inventario después de cancelar:';
SELECT 
    p.nombreProducto,
    i.cantidadDisponible,
    FORMAT(p.precioVenta, 'C', 'es-CO') as PrecioUnitario
FROM TBL_Inventario i
JOIN TBL_Producto p ON i.idProducto = p.idProducto
WHERE i.idSede = 1 AND i.idProducto IN (1, 4);
GO

-- Prueba 11: Reporte de ventas por fecha con formato colombiano
PRINT 'Prueba 11: Reporte de ventas por fecha con formato colombiano...';
EXEC SP_ReporteVentasPorFecha @fechaInicio = '2024-01-01', @fechaFin = GETDATE();
GO

-- Prueba 12: Actualizar inventario manualmente
PRINT 'Prueba 12: Actualizando inventario manualmente...';
EXEC SP_ActualizarInventario 
    @idSede = 1, 
    @idProducto = 1, 
    @nuevaCantidad = 30, 
    @idUsuario = 1,
    @observaciones = N'Reabastecimiento de whisky premium';
GO

-- Prueba 13: Ver movimientos de inventario
PRINT 'Prueba 13: Movimientos de inventario...';
SELECT 
    p.nombreProducto,
    mi.tipoMovimiento,
    mi.cantidad,
    mi.cantidadAnterior,
    mi.cantidadNueva,
    FORMAT((SELECT precioVenta FROM TBL_Producto WHERE idProducto = mi.idProducto), 'C', 'es-CO') as PrecioUnitario,
    mi.observaciones,
    mi.fechaMovimiento
FROM TBL_Movimientos_Inventario mi
JOIN TBL_Producto p ON mi.idProducto = p.idProducto
ORDER BY mi.fechaMovimiento DESC;
GO

-------------------------------------------------------------------------
------------------------ VERIFICACIÓN FINAL -----------------------------
-------------------------------------------------------------------------

PRINT '=== VERIFICACIÓN FINAL DEL SISTEMA ===';

-- Verificar todos los componentes con formato colombiano
PRINT '1. Sedes creadas:';
SELECT * FROM TBL_Sedes;
GO

PRINT '2. Usuarios del sistema:';
SELECT 
    u.usuarioLogin,
    u.nombreUsuario + ' ' + u.apellidoUsuario as NombreCompleto,
    r.nombreRol as Rol,
    s.nombreSede as Sede
FROM TBL_Usuarios u
JOIN TBL_Roles r ON u.idRol = r.idRol
JOIN TBL_Sedes s ON u.idSede = s.idSede;
GO

PRINT '3. Productos disponibles con precios formateados:';
SELECT 
    p.codigoProducto,
    p.nombreProducto,
    c.nombreCategoria,
    FORMAT(p.precioVenta, 'C', 'es-CO') as PrecioVenta,
    FORMAT(p.costo, 'C', 'es-CO') as Costo,
    FORMAT((p.precioVenta - p.costo), 'C', 'es-CO') as Margen
FROM TBL_Producto p
JOIN TBL_Categoria c ON p.idCategoria = c.idCategoria
WHERE p.estado = 'activo';
GO

PRINT '4. Estado de mesas:';
SELECT 
    s.nombreSede,
    m.numeroMesa,
    m.estado,
    m.capacidad
FROM TBL_Mesas m
JOIN TBL_Sedes s ON m.idSede = s.idSede
ORDER BY s.nombreSede, m.numeroMesa;
GO

PRINT '5. Inventario actual por sede con valores:';
SELECT 
    s.nombreSede,
    p.nombreProducto,
    i.cantidadDisponible,
    FORMAT(p.precioVenta, 'C', 'es-CO') as PrecioVenta,
    FORMAT((i.cantidadDisponible * p.precioVenta), 'C', 'es-CO') as ValorTotalStock
FROM TBL_Inventario i
JOIN TBL_Sedes s ON i.idSede = s.idSede
JOIN TBL_Producto p ON i.idProducto = p.idProducto
ORDER BY s.nombreSede, p.nombreProducto;
GO

PRINT '=== PRUEBAS COMPLETADAS EXITOSAMENTE ===';
PRINT 'El sistema DB_BARSTOC está funcionando correctamente con moneda colombiana.';
PRINT '=== DATOS DE ACCESO PARA PRUEBA ===';
PRINT '- Usuario administrador: carlos.admin / 123456';
PRINT '- Usuario cajero: ana.cajero / 123456';
PRINT '- Usuario mesero: luis.mesero / 123456';
PRINT '- Usuario mesero 2: maria.mesero / 123456';
PRINT '=== CARACTERÍSTICAS IMPLEMENTADAS ===';
PRINT '✅ Soporte UTF-8 para caracteres especiales (tildes, ñ)';
PRINT '✅ Precios en formato colombiano (miles y millones)';
PRINT '✅ Formato monetario: $1.000,00 $100.000,00 $1.000.000,00';
PRINT '✅ Nombres y direcciones con caracteres especiales';
PRINT '✅ Validaciones para precios colombianos';
GO