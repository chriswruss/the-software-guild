$(document).ready (function() {
    loadItems();

    $('#moneyAdded').val('0.00'); // Setting the initial value of moneyAdded to 0.00

    // The "Make Purchase" button. The button is disabled until an item has been 
    // selected and money has been added to the machine. 
    $('#makePurchase').click(function(event) {
        var money = parseFloat($('#moneyAdded').val()); // Grab the current value in the "Total $ In"
        money = money.toFixed(2); // Set it to two decimal places

        // Handle the GET request
        $.ajax({
            type: 'GET',
            url: 'http://localhost:8080/money/' + money + '/item/' + $('#itemNumberSelected').text(),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            'dataType': 'json',
            success: function(changeReturned) {
                printUserChange(changeReturned); // printUserChange takes the change in the response and prints it to the change return window
                $('#messageBox').text('THANK YOU!');
                $('#itemNumberSelected').empty();
                loadItems(); // Reload all of the items to show updated quantities
            },
            error: function(response) {
                $('#messageBox').text(response.responseJSON.message); // Add the error message in the response to the messages window
            }
        });
    });
});

// Uses a GET method to load in all of the items on the vending machine web server
function loadItems() {
    $.ajax({
        type: 'GET',
        url: 'http://localhost:8080/items',
        success: function(itemArray) {
            // Grab each item from the JSON array and assign it to the appropriate value
            $.each(itemArray, function(index, item) {
                var id = item.id;
                var name = item.name;
                var price = item.price;
                var quantity = item.quantity;
                var itemId = "item" + id;
                var idNumber = $('#' + itemId + 'Number');
                var idTitle = $('#' + itemId + 'Title');
                var idPrice = $('#' + itemId + 'Price');
                var idQuantity = $('#' + itemId + 'QuantityRemaining');

                // Empty out all of product information
                idNumber.empty();
                idTitle.empty();
                idPrice.empty();
                idQuantity.empty();

                // Add all of the new items from the response
                idNumber.text(id);
                idTitle.append('<a onclick="displayProductInItemWindow(' + id + ')">' + name + '</a>');
                price = convertToCurrency(price).toString();
                idPrice.text('Price: $' + price);
                idQuantity.text('Quantity Left: ' + quantity);
            });
        },
        error: function() {
            $('#errorMessages')
                .append($('<li>')
                .attr({class: 'list-group-item list-group-item-danger'})
                .text('Error calling web service. Please try again later.'));
        }
    });
}

// Function is called by the title in each item window
function displayProductInItemWindow(itemId) {
    // Empty out all of the boxes with messages first
    $('#itemNumberSelected').empty();
    $('#messageBox').empty();
    $('#amountChangeReturned').empty();
    $('#itemNumberSelected').append(itemId);
    // Check if any money has been deposited
    if ($('#moneyAdded').text() != ''){
        $('#makePurchase').prop('disabled', false);
    }
}

// Converts a string into a two digit string
function convertToCurrency(price) {
    price = parseFloat(price);
    price = price.toFixed(2);
    return price.toString();
}

// Function that gets called when user clicks to add money
// Money values are hard coded in the home.html file
function addToTotal(number) {
    $('#messageBox').empty();
    $('#messageBox').text(' ');
    var retVal = parseFloat($('#moneyAdded').val()); // Grabs the value inside of the moneyAdded id and converts it from string to float
    if (retVal == 0) {
        $('#changeReturnButton').prop('disabled', false);
        $('#amountChangeReturned').empty();
    }
    if ($('#itemNumberSelected').text() != ''){
        $('#makePurchase').prop('disabled', false);
    }
    retVal += number;
    $('#moneyAdded').val(retVal);
    $('#moneyAdded').empty();
    retVal = convertToCurrency(retVal); // Send the retVal to be converted to a 2 digit number, then converted to a string before being sent back
    $('#moneyAdded').append(retVal); // Print the updated value into the total box
}

// Handles the "Change Return" button.
function changeReturnHandler() {
    var val = parseFloat($('#moneyAdded').val());
    val = val.toFixed(2);
    var quarters = 0;
    var dimes = 0;
    var nickels = 0;
    var changeDue = "";
    if (val >= 0.25) {
        quarters = getChangeCoins(val, 0.25);
        val -= (quarters * 0.25);
        val = val.toFixed(2);
        changeDue += quarters + " Quarter(s)";
    }
    if (val >= 0.10) {
        dimes = getChangeCoins(val, 0.10);
        val -= (dimes * 0.10).toFixed(2);
        val = val.toFixed(2);
        changeDue += " " + dimes + " Dime(s)";
    }
    if (val != 0) {
        nickels = getChangeCoins(val, 0.05);
        val -= (nickels * 0.05).toFixed(2);
        val = val.toFixed(2);
        changeDue += " " + nickels + " Nickel(s)";
    }

    $('#amountChangeReturned').text(changeDue);

    // Empty out all of the windows with messages
    $('#moneyAdded').val('0.00');
    $('#moneyAdded').empty();
    $('#itemNumberSelected').empty();
    $('#messageBox').empty();
    $('#changeReturnButton').prop('disabled', true);
    $('#makePurchase').prop('disabled', true);
}

// Used by the "Change Return" button to take the money added and parse it 
// back into Quarters, Dimes and Nickels
function getChangeCoins(total, coinValue) {
    var retVal = 0;
    while (total >= coinValue) {
        total -= coinValue;
        retVal++;
    }
    return retVal;
}

// Prints the change that was returned in the response message from clicking
// the "Make Change" button.
function printUserChange(response){
    var val = '';
    if(response.quarters != '0'){
        val += response.quarters + ' Quarter(s) ';
    }
    if(response.dimes != '0') {
        val += response.dimes + ' Dime(s) ';
    }
    if(response.nickels != '0') {
        val += response.nickels + ' Nickel(s)';
    }

    $('#amountChangeReturned').text(val);

    // Empty all of the windows.
    $('#moneyAdded').val('0.00');
    $('#moneyAdded').empty();
    $('#changeReturnButton').prop('disabled', true);
    $('#makePurchase').prop('disabled', true);
} 