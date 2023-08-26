select * from Pedido

select * from Cliente

select * from Cliente, Pedido where Cliente.id = Pedido.cCliente

select * from Cliente as c, Pedido as p where c.poblacion = 'Sincelejo' and c.id = p.cCliente 
SELECT CONCAT (cCliente, ' ', fechaPedido,' ',formaPago) as concatenado FROM PEDIDO