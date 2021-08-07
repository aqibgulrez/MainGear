<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="SCE.MainGear.API.ProductForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Service</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
             <input type="text" name="ProductID" id="ProductID"/>
             <input type="button" id="btnGetProduct" value="Get Product"/>
             <input type="button" id="btnPutProduct" value="Put Product"/>
        </div>
        <hr/>
        <div>
             <input type="button" id="btnPlaceOrder" value="PlaceOrder"/>
        </div>
        <pre ID="JSON">

        </pre>
        <pre ID="JSON2">

        </pre>
        <pre ID="placeOrderJson">

        </pre>
    </div>
    </form>
    <script type="text/javascript">
        $(function () {
            var json;
            var jsonString;
            $('#btnGetProduct').on('click', function () {
                var options = {
                    type: "POST",
                    url: "<%= ResolveUrl("~/product.aspx/GetProductJSON") %>",
                    data: '{ProductID:"' + $('#ProductID').val() + '"}',
                    async: false,
                    cache: false,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response != null && response.d != null) {
                            var data = response.d;
                            //we need to parse it to JSON 
                            json = $.parseJSON(data);
                            console.log(json);
                            jsonString = JSON.stringify(json);
                            $("#JSON").html(htmlEntities(jsonString));
                        }
                    }
                };
                $.ajax(options);
            });


            $('#btnPutProduct').on('click', function () {
                $("#JSON2").html('{Product:"' + jsonString + '"}');
                var options = {
                    type: "POST",
                    url: "<%= ResolveUrl("~/product.aspx/PutProductJSON") %>",
                    data: "{lProduct:'" + jsonString + "'}",
                    async: false,
                    cache: false,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response != null && response.d != null) {
                            var data = response.d;
                            json = $.parseJSON(data);
                            jsonString = JSON.stringify(json);
                            $("#JSON2").html(htmlEntities(jsonString));
                        }
                    }
                };
                $.ajax(options);
            });

 
            var ProductOrderJSON = '{"Items":[{"SectionItemID":10020,"Quantity":1,"ItemPrice":"429.80"},{"SectionItemID":10013,"Quantity":1,"ItemPrice":"429.80"},{"SectionItemID":10025,"Quantity":1,"ItemPrice":"252.00"}],"TotalPrice":"1111.58","EstimatedShippingDate":"November 3, 2017"}';
            var OrderJson = "{ProductOrderJSON:'" + ProductOrderJSON + "'}";
            $("#placeOrderJson").html(OrderJson);
            $('#btnPlaceOrder').on('click', function () {
                var options = {
                    type: "POST",
                    url: "<%= ResolveUrl("~/product.aspx/PlaceOrder") %>",
                    data: "{ProductOrderJSON:'" + ProductOrderJSON + "'}",
                    async: false,
                    cache: false,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (response != null && response.d != null) {
                            var data = response.d;
                            //we need to parse it to JSON 
                            var ljson = $.parseJSON(data);
                            ProductOrderJSON = JSON.stringify(ljson);
                            $("#placeOrderJson").html(ProductOrderJSON);
                        }
                    }
                };
                $.ajax(options);
            });

            function htmlEntities(str) {
                return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
            }

        });
    </script>
</body>
</html>
