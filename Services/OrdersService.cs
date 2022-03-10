using WarriorSalesAPI.DTOs;
using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Services
{
    public class OrdersService
    {
        public static List<OrderListDTO> GenerateListOfOrderListDTO(List<Order> orders)
        {
            List<OrderListDTO> ordersDTO = new();

            foreach (var order in orders)
            {
                OrderListDTO orderListDTO = new()
                {
                    Address = order.Address,
                    Creation = order.Creation,
                    Delivery = order.Delivery,
                    Team = order.Team,
                };

                ordersDTO.Add(orderListDTO);
            }

            return ordersDTO;
        }

        public static GenericResult GenerateOrder(List<Team> teams, AddOrderDTO addOrderDTO)
        {
            var result = new GenericResult() { Error = false, Message = "" };
            int teamsCount = teams.Count;

            if (teamsCount == 0)
            {
                result.Error = true;
                result.Message = "No team avaiable to execute the order.";
            }

            Team randomTeam = teams[new Random().Next(teamsCount)];
            var order = new Order { Address = addOrderDTO.Address, Team = randomTeam };

            result.Result = order;

            return result;
        }

        public static GenericResult ValidateSaleItemWithProduct(
            CartItem cartItem,
            Product product)
        {
            var result = new GenericResult() { Error = false, Message = "" };

            if (product == null)
            {
                result.Error = true;
                result.Message = $"Product named {cartItem.Name} was not found.";
            } 
            
            else if (cartItem.Quantity > product.Stock)
            {
                result.Error = true;
                result.Message = $"Product named {cartItem.Name} has an insuficient stock of {product.Stock} items.";
            }

            return result;
        }

        public static SaleItem GenerateSaleItem(Product product, Order order, int quantity)
        {
            return new()
            {
                Name = product.Name,
                Description = product.Description,
                Order = order,
                Price = product.Price,
                Quantity = quantity,
                Product = product,
            };
        }
    }

    public class GenericResult
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public dynamic? Payload { get; set; }
    }
}
