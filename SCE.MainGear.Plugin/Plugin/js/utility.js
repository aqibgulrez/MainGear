////////////////////////////////////////////////////////////////// GLOBAL VARIABLES >>/////////////////////////////////////////////////////////
WIDGET_UTILITY_LOADED = true;
sum = 0;
WIDGET_CACHE_BUST = "";

////////////////////////////////// MAIN GEAR START///////////////////////////////////////

//Get the widget container
function maingear_getWidgetContainer() {
  $container = $(WIDGET_CONTAINER_ID);
  $containerRoot = $container.find("#widget_root");
  return $containerRoot.find("#widget_container");
}



//Generic method to load a template, pass the relative path and the JSON object (no json string please)
function maingear_loadTemplate($object, $path, $json, loadCallBack) {
  $object.setTemplateURL(WIDGET_ROOT_URL + $path, [], { filter_data: false });
  $object.processTemplate($json);
  if (loadCallBack) {
    loadCallBack();
  }
}

//fetching json
function getJSON() {
  // cacheBust();
  console.log('JSON URL= ' + JSON_URL);
  var options = {
    type: "POST",
    url: JSON_URL + "product.aspx/GetProductJSON",
    data: '{ProductID:"' + PRODUCT_ID.toString() + '"}',
    async: false,
    cache: false,
    headers: { "cache-control": "no-cache" },
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function(response) {
      if (response != null && response.d != null) {
        var data = response.d;
        JSON_MAINGEAR = $.parseJSON(data);
        jsonString = JSON.stringify(JSON_MAINGEAR);

      }
    }
  };
  $.ajax(options);
}
//Load Products from Local JSON
function maingear_LoadProducts_local(isNew) {
  maingear_LoadProductsData(JSON_MAINGEAR, isNew);

}
//Load Products from api call
function maingear_LoadProducts_get(isNew) {

  getJSON();
  console.log(JSON_MAINGEAR);
  maingear_LoadProductsData(JSON_MAINGEAR, isNew);

}


function maingear_LoadProductsData(data, isNew) { //Jtemplate call, and initial calculations
  $json = data;
  $container = $(WIDGET_CONTAINER_ID);
  $containerRoot = $container.find("#widget_root");
  $widget_container = $containerRoot.find("#widget_container");

  setTimeout(function() {
    $containerRoot.find(".loader").hide();
    maingear_loadTemplate($widget_container, "html/design.html", $json, function() {
      init_html_components();
      show_checked_spinners();
      move_spinner_beneath_item();
      calculate_Loader_Length();
      showOnlyFirst_ProductModeImage();
      onLoad_Price();
      init_Total();
      calculate_Date();
      onLoad_check_choices(); //check if onload the choice item is checked then update the itemid of the input
      check_filters_for_each();
    });
  }, 1000);
}

function maingear_LoadProducts() {
  if ($("body #refbodytop").length == 0) {
    $("body").prepend("<a id='refbodytop' name='btop'></a>"); //Reference for mobile scrolling...
    console.log("Taking JSON from " + WIDGET_REQUEST_MODE);

  }
  if (WIDGET_REQUEST_MODE == "local") {
    maingear_LoadProducts_local(true);
  } else if (WIDGET_REQUEST_MODE == "get") {
    maingear_LoadProducts_get(true);
  }
}
/////////////////////////Other functions////////////////
var prevItemLabel;
var resetPrice = 0;

//We need this to avoid any kind of caching on client side for html templates
function cacheBust(dynamic) {

  if (dynamic || WIDGET_CACHE_BUST == "-1") {
    return new Date().getMilliseconds().toString();
  } else {
    if (WIDGET_CACHE_BUST == "") {
      $.get(WIDGET_ROOT_URL + 'API.aspx?ops=GetCacheBust', function(result) {
        result = JSON.parse(result);
        WIDGET_CACHE_BUST = result.cacheBust;
      });
    }

    return WIDGET_CACHE_BUST;
  }
}

function htmlEntities(str) { //covert the json content to renderable html
  return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}

function init_html_components() { //Initialize diffrerent elements like spinner , tooltip etc
  $('.spinner-input').each(function() {
    if ($(this).hasClass('spinner-step')) {
      $(this).find('input').spinner({
        step: $(this).data('step'),
        min: 1,
        incremental: false,
        spin: function(event, ui) {
          clicked_spinner = $(this);
          if ($(this).val() < ui.value) {
            // new value = ui.value
            increase_Total(clicked_spinner, ui.value);
          } else {
            $(this).val()
            // previous value = $(this).val()
            decrease_Total(clicked_spinner, $(this).val());
          }
        }
      });
    } else {
      $(this).find('input').spinner({
        step: 1
      });
    }
  });

  $('[data-toggle="tooltip"]').tooltip();

  $('.popup-inner .instructions-wrap').each(function() {
    $(this).html($(this).text());
  });

}

function calculate_Loader_Length() { //calculate length of red bar beneath steps
  var totalSteps = $('div[id^="Step"]').length;
  var w = 0;
  $('.loader-bar').each(function() {
    var stepIndex = $(this).closest('div[id^="Step"]').index();
    w = stepIndex / totalSteps * 100;
    $(this).css('width', w + '%');
  });
}

function showOnlyFirst_ProductModeImage() {
  $('div[id^="Step"] .product-view .img-column .img-wrap').hide();
  $('div[id^="Step"] .product-view:first-child .img-column .img-wrap').show();
}

$('body').on('click', '.toggle-relative,.toggle-absolute', function() { //toggle prices
  if ($(this).hasClass('toggle-relative')) {
    $('.product-view,.build-box').each(function() {
      if ($(this).data('sectionpricing') == 1) {
        $(this).find('input[type="radio"]~.price-shor').toggleClass('hide');
      }
    });
  } else {
    $('.product-view,.build-box').each(function() {
      if ($(this).data('sectionpricing') == 1) {
        $(this).find('input[type="checkbox"]~.price-shor').toggleClass('hide');
      }
      if ($(this).data('sectionpricing') == 2) {
        $(this).find('.price-shor').toggleClass('hide');
      }
    });
  }
});


/////////////////////////Shipping Date Functions////////////////

function update_Date(month, day, year) {
  $(".estdate").html(month + " " + day + ", " + year);
}

function calculate_Date() { //calculate time for longest processing
  var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
  var d = new Date();
  var longestProcessing = 0;

  $('input:checked').each(function() {
    var temp = $(this).data('processingtime');
    if (temp > longestProcessing) {
      longestProcessing = temp; //longest processing time
    }
  });
  console.log('LongestProcessing=' + longestProcessing);
  d.setDate(d.getDate() + longestProcessing); //sets estimated date
  update_Date(monthNames[d.getMonth()], d.getDate(), d.getFullYear());
}

/////////////////////////Pricing Functions////////////////

function uptil_2_deci(e) { //restrict decimal points upto 2
  // if (e % 1 != 0) {
  //   // return parseFloat(e.toFixed(2));
  // } else {
  //   return e;
  // }

  // return ((Math.floor(e * 100) / 100).toFixed(2));

  return (((e * 100) / 100).toFixed(2));
}

function update_Total() {
  var temp = uptil_2_deci(sum);
  $(".right-container").find("h4#subtotal").html("$" + temp);
}

function init_Total() {
  if (resetPrice == 1) {
    sum = 0;
  }
  $('input').each(function() {
    if (!$(this).hasClass('ignore')) { //ignore toggle price checboxes clicks
      if ($(this).is(':checked')) {
        // console.log(parseFloat($(this).data('price')));
        sum = sum + parseFloat($(this).data('price'));
      }
    }
  });
  update_Total();
  resetPrice = 0;
}


function onLoad_Price() { //calculate relative pricing on page load
  $('.product-view,.build-box').each(function() {
    if ($(this).data('sectionpricing') == 1) {
      $(this).find('.item-wrap').each(function() {
        checked_item = $(this).find('input:checked');
        if (checked_item.val() == "on") { //check if item-wrap contains atleast one checked item
          $(this).find('input').each(function() {
            if ($(this).attr('type') == 'radio') { //if radios are in relative section then show relative pricing
              if (!$(this).is(':checked')) {
                other_item = $(this);
                if (parseFloat(other_item.data('price')) > parseFloat(checked_item.data('price'))) {
                  // show positive
                  other_item_price = parseFloat(other_item.data('price') - checked_item.data('price'));
                  other_item_price = uptil_2_deci(other_item_price);

                  other_item.nextAll('.price-shor').eq(0).text('[+$' + other_item_price + ']');
                } else {
                  // show negative
                  other_item_price = parseFloat(checked_item.data('price') - other_item.data('price'));
                  other_item_price = uptil_2_deci(other_item_price);
                  other_item.nextAll('.price-shor').eq(0).text('[-$' + other_item_price + ']');
                }
              }
            } else if ($(this).attr('type') == 'checkbox') { //if checkbox is in relative section show original price
              $(this).nextAll('.price-shor').eq(0).text('[$' + $(this).data('price') + ']');
              if ($(this).is(':checked')) {
                $(this).nextAll('.price-shor').hide();
              }
            }

          });
        }
      });
    } else if ($(this).data('sectionpricing') == 2) { //calculate absolute pricing of radios and checkboxes
      $(this).find('input[type="checkbox"],input[type="radio"]').each(function() {
        $(this).nextAll('.price-shor').eq(0).text('[$' + $(this).data('price') + ']');
        if ($(this).is(':checked')) {
          $(this).nextAll('.price-shor').hide();
        }
      });
    }
  });

}


function calculate_Relative_Price(e) { //calculate relative pricing on input change
  e.find('.price-shor').hide();
  onLoad_Price();
  if (!e.find('input').is(':checked')) { //show price on checkbox uncheck
    e.find('.price-shor').show();
  }
}


function show_Other_Relative_Price(e) { //show relative price of other items in the same sections
  e.closest('.item-wrap').find('label input').each(function() {
    if (!$(this).is(e) && !$(this).is(':checked')) {
      $(this).closest('label').find('.price-shor').show();
    }
  });
}

function check_Total(e) {
  item = e.find('input');
  item_price = parseFloat(item.data('price'));
  if (e.hasClass('label-with-spinner')) { // if spinner exist multiply values
    spinner_val = e.prev('.spinner-input').find('input').val();
    if (spinner_val != "") {
      item_price = item_price * spinner_val;
    }
  }
  if (e.find('input[type="radio"]').length > 0 && prevVal != null) {
    // console.log('Previous val=' + prevVal);
    sum = sum - prevVal; //subtract value of previous selected item
  }
  if (item.is(':checked')) {
    console.log('Adding: ' + item_price);
    sum = sum + item_price;
    console.log('New Total: ' + sum);
    update_Total();
  } else {
    console.log('Substracting: ' + item_price);
    sum = parseFloat(Number(sum - item_price).toFixed(2)); //used toFixed method to give accurate substraction result 
    console.log('New Total: ' + sum);
    update_Total();

  }
}

function increase_Total(obj, multiplier) { //increase total when spinner is incremented
  if (multiplier > 1 && obj.closest('.spinner-input').next('label').find('input').is(':checked')) {
    objPrice = obj.closest('.spinner-input').next('label').find('input:checked').data('price');
    objPrice_increment = obj.closest('.spinner-input').data('step');
    increment_price = objPrice * objPrice_increment;
    sum = sum + increment_price;
    update_Total();
  }
}

function decrease_Total(obj, multiplier) { //decrease total when spinner is decremented
  if (multiplier > 1 && obj.closest('.spinner-input').next('label').find('input').is(':checked')) {
    objPrice = obj.closest('.spinner-input').next('label').find('input:checked').data('price');
    objPrice_increment = obj.closest('.spinner-input').data('step');
    decrement_price = objPrice * objPrice_increment;
    sum = sum - decrement_price;
    update_Total();

  }
}

function get_all_checked_values() { //generic function to get price of all checked items
  $('input:checked').each(function() {
    if ($(this).closest('label').hasClass('label-with-spinner')) {
      var x = $(this).closest('label').prev('.spinner-input').find('input').val();
      var y = $(this).data('price') * x;
      console.log($(this).data('price') + ' x ' + x + ' = ' + y);
    } else {
      console.log($(this).data('price'));
    }
  });
}
/////////////////////////Filters and Tags Functions////////////////

function getTags(e) {
  var str = e.find('input:checked').data('tags');
  str = str.split('|');
  return str.filter(function(v) {
    return v !== ''
  });
}

function getFilters(e) {
  var str = e.find('input:checked').data('filters');
  if (e.find('input').attr('type') == 'checkbox') {
    str = e.find('input').data('filters');
  }
  if (e == prevItemLabel) {
    str = e.find('input').data('filters');
  }
  // console.log('filter text=' + str);
  if (str != undefined && str.length > 0) {
    str = str.split('|');
    return str.filter(function(v) {
      return v !== ''
    });
  }
}

function splitFilter(e) {
  var str = e.split('_');
  return str.filter(function(v) {
    return v !== ''
  });
}

function show_Original_Price(sectionName) { //for absolute pricing
  $("div").find("[data-sectionname='" + sectionName + "']").find('input').each(function() {
    $(this).nextAll('.price-shor').eq(0).text('[$' + $(this).data('price') + ']');
  });
}

function apply_Filter(x, y, z) {
  var sectionName = x;
  var tagName = y;
  var filerType = z;
  var multiplier = 1;
  var sectionDiv = $("div").find("[data-sectionname='" + sectionName + "']");
  var section_inputs = $("div").find("[data-sectionname='" + sectionName + "']").find('label:not(".current-node") input');
  section_inputs.each(function() {
    if ($(this).attr('data-tags') && $(this).attr('data-itemid') != $('.last-clicked').attr('data-itemid')) {
      var item_tags = $(this).attr('data-tags');
      if (item_tags.indexOf(tagName) >= 0) {
        if (filerType == "Contains Disable") {
          console.log('Filter Applied: Disabled');

          if ($(this).is(':checked')) {
            if ($(this).closest('label').hasClass('label-with-spinner')) {
              multiplier = $(this).closest('label').prev('.spinner-input').find('input').val();
            }
            sum = sum - $(this).data('price') * multiplier;
            update_Total();

            if ($(this).attr('type') == 'radio') { //for radios only
              show_Original_Price(sectionName);
            }
          }
          $(this).prop('checked', false);
          $(this).closest('label').addClass('disabled');
          $(this).closest('label').removeClass('active');
          hide_checkbox_spinner($(this));
          $(this).closest('label').next('.multi-BTO-select').addClass('hide');

          calculate_Date();
        } else if (filerType == "Contains Hidden") {
          console.log('Filter Applied: Hidden');

          if ($(this).is(':checked')) {
            if ($(this).closest('label').hasClass('label-with-spinner')) {
              multiplier = $(this).closest('label').prev('.spinner-input').find('input').val();
              // console.log(multiplier);
            }
            sum = sum - $(this).data('price') * multiplier;
            update_Total();
            if ($(this).attr('type') == 'radio') { //for radios only
              show_Original_Price(sectionName);
            }
          }
          $(this).prop('checked', false);
          $(this).closest('label').addClass('disabled');
          $(this).closest('label').hide();
          $(this).closest('label').removeClass('active');
          hide_checkbox_spinner($(this));
          $(this).closest('label').next('.multi-BTO-select').addClass('hide');

          calculate_Date();
        }
        if (!(sectionDiv.find('input').is(':checked'))) {
          show_Original_Price(sectionName);
          //////////////// this is to check the default item of the group ////////////////

          // sectionDiv.find('.default-item input').prop('checked', true);
          // resetPrice = 1;
          // init_Total();

          //////////////// this is to check the default item of the group ////////////////
        }
      }
    }
  });
}

function check_filters(e) {
  allFilters = getFilters(e);
  e.addClass('current-node');
  // console.log(allFilters);
  $.each(allFilters, function(index, value) {
    singleFilter = splitFilter(value);
    $.each(singleFilter, function(index, value) {
      var sectionName = singleFilter[0];
      var tagName = singleFilter[1];
      var filerType = singleFilter[2];
      if (index == 2) {
        apply_Filter(sectionName, tagName, filerType);
      }
    });
  });

  e.removeClass('current-node');

}

function check_filters_for_each() { //loop through each item and apply filters accordingly
  // console.log('check_filters_for_each called');
  $('input:checked').each(function() {
    check_filters($(this).closest('label'));
  });
}

function calculate_Price_Relative_To_Checked(e) { //Price calculation for relative pricing
  var other_item = e;
  var checked_item = e.closest('.item-wrap').find('input:checked');
  if (checked_item.val() == "on") { //check if item-wrap contains atleast one checked item
    if ($(this).attr('type') == 'radio') {
      //if radios are in relative section then show relative pricing
      if (other_item.data('price') > checked_item.data('price')) {
        // show positive
        other_item_price = parseFloat(other_item.data('price') - checked_item.data('price'));
        other_item_price = uptil_2_deci(other_item_price);
        other_item.nextAll('.price-shor').eq(0).text('[+$' + other_item_price + ']');
      } else {
        // show negative
        other_item_price = parseFloat(checked_item.data('price') - other_item.data('price'));
        other_item_price = uptil_2_deci(other_item_price);
        other_item.nextAll('.price-shor').eq(0).text('[-$' + other_item_price + ']');
      }
    }
  }
}

function remove_Filter(x, y, z) {
  console.log('remove_Filter called');
  var sectionName = x;
  var tagName = y;
  var filerType = z;
  var section_inputs = $("div").find("[data-sectionname='" + sectionName + "']").find('input');
  section_inputs.each(function() {
    if ($(this).attr('data-tags')) {
      var item_tags = $(this).attr('data-tags')
      if (item_tags.indexOf(tagName) >= 0) {
        console.log('found the previous item filter');
        console.log('Filter removed');
        $(this).closest('label').removeClass('disabled');
        $(this).closest('label').show();
        if ($(this).attr('type') == 'radio') { // for radios only
          calculate_Price_Relative_To_Checked($(this));
        }
      }
    }
  });
}

function check_prev_filters(e) { //check filters of previous checked item and reverse the filter applied
  // console.log('check_prev_filters called');
  if (prevItemLabel != undefined || e.find('input').attr('type') == 'checkbox') {
    allFilters = getFilters(e);
    $.each(allFilters, function(index, value) {
      singleFilter = splitFilter(value);
      $.each(singleFilter, function(index, value) {
        var sectionName = singleFilter[0];
        var tagName = singleFilter[1];
        var filerType = singleFilter[2];
        if (index == 2) {
          remove_Filter(sectionName, tagName, filerType);
        }
      });
    });
  }
}

/////////////////////////Spinner Functions////////////////

function show_checked_spinners() { //show spinners of all checked items
  $('.label-with-spinner.active').each(function() {
    $(this).prev('.spinner-input').show();
  });
}

function move_spinner_beneath_item() {
  $('.spinner-input').each(function() { //for list view
    if ($(this).is(':visible')) {
      $(this).css('top', $(this).next('.label-with-spinner').outerHeight());
      $(this).next('.label-with-spinner').css('top', -$(this).outerHeight());
    }
  });
  $('.product-view .spinner-input').each(function() { //for product view
    if ($(this).is(':visible')) {
      $(this).css('top', $(this).next('.label-with-spinner').outerHeight());
      $(this).next('.label-with-spinner').css('top', -$(this).outerHeight() - 10);
    }
  });
}

function remove_position(e) { //reset position of the spinners 
  prevItemLabel.css('top', '0');
  prevItemLabel.prev('.spinner-input').css('top', '0');
}

function hide_checkbox_spinner(e) {
  if (e.closest('label').hasClass('label-with-spinner')) {
    e.closest('label').prev('.spinner-input').hide();
    e.closest('label').css('top', '0');
  }
}
$(window).resize(function() { //on resize update the spinner postion values
  move_spinner_beneath_item();
});

function check_spinner(e) { //check positions of spinner and move the spinners beneath item
  if (e.hasClass('label-with-spinner')) {
    e.prev('.spinner-input').show();
    move_spinner_beneath_item();
  }
  if (prevItemLabel == undefined || prevItemLabel == '') {
    console.log('prevItemLabel not defined');
    return;
  }
  if (e.find('input').attr('type') == 'radio' && prevItemLabel.hasClass('label-with-spinner')) //hide spinner if unchecked
  {
    prevItemLabel.prev('.spinner-input').hide();
    remove_position(); //reset position of the spinners
  }
}

/////////////////////////Finish button and place order////////////////


function finish() {
  $('.loader-wrap').css('display', 'block');
  $.notifyClose(); //close all notifiers
  myinterval = setInterval(function() { //show loader for a second
    placeOrder();
    clearInterval(myinterval);
  }, 1100);
}

function placeOrder() {
  // check if all required sections have atleast one checked item
  if (!validateRequiredSections()) {
    return;
  }
  var jsonObj = new Object();
  var items = [];
  var orderItemObj = new Object();
  var coma = "";
  var x;
  var count = 0;
  $('input:checked').each(function() {
    if (!$(this).hasClass('ignore')) { //ignore toggle price checboxes clicks
      orderItemObj = new Object();
      orderItemObj["SectionItemID"] = $(this).attr('data-itemid');
      orderItemObj["SectionItemOptionID"] = $(this).attr('data-sectionitemoptionid');
      if ($(this).closest('label').hasClass('label-with-spinner')) {
        orderItemObj["Quantity"] = parseFloat($(this).closest('label').prev('.spinner-input').find('input').val());
      } else {
        orderItemObj["Quantity"] = 1;
      }
      orderItemObj["ItemPrice"] = ((parseFloat($(this).data('price')) * 100) / 100).toFixed(2);
      items[count] = orderItemObj;
      count++;
    }
  });
  jsonObj.Items = items;
  jsonObj.TotalPrice = ((parseFloat($('#subtotal').text().split('$')[1]) * 100) / 100).toFixed(2);
  jsonObj.EstimatedShippingDate = $('.estdate:first').text();
  jsonObj = JSON.stringify(jsonObj);
  console.log(jsonObj);

  var options = {
    type: "POST",
    url: JSON_URL + "product.aspx/PlaceOrder",
    data: "{ProductOrderJSON:'" + jsonObj + "'}",
    async: false,
    cache: false,
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function(response) {
      if (response != null && response.d != null) {
        var data = response.d;
        //we need to parse it to JSON 
        var ljson = $.parseJSON(data);
        ProductOrderJSON = JSON.stringify(ljson);
        console.log(data);
      }
    }
  };
  $.ajax(options);

  $.notify({
    message: 'Order Placed SuccessFully'
  }, {
    type: 'success',
    delay: 2000
  });
}

function validateRequiredSections() { //validates that required sections should contain atleast one checked item
  $('.loader-wrap').css('display', 'none');
  var count = 0;
  msgstr = "";
  var x, y;
  $('*[data-sectionrequired="true"]').each(function() {
    x = $(this).data('sectionname');
    msgstr = 'Please select atleast one option from Section <strong>' + x + '</strong>';
    $(this).find('*[data-subsectionname]').each(function() {
      y = $(this).data('subsectionname');
      if ($(this).data('subsectionname').length > 0) {
        msgstr = 'Please select atleast one item from subsection <strong>' + y + '</strong> in Section <strong>' + x + '</strong>';
      }
      if (!$(this).find('input:checked').length > 0) {
        $.notify({
          message: msgstr,
        }, {
          type: 'danger',
          delay: 10000,
        });
        count = 1;
      }
    });
  });
  if (count == 0) {
    return true;
  } else
    return false;
}

/////////////////////////Choice Functions/////////////////

function update_sectionitemoptionid(e) { //updates dataitem id of the label according to the choice selected
  var newitemID = e.find('option:selected').attr('sectionitemoptionid');
  e.prev('label').find('input').attr('data-sectionitemoptionid', newitemID);
}

function check_choices(e) {
  if (e.data('choices')) { //find labels which has datachoices true
    update_sectionitemoptionid(e.next('.multi-BTO-select'));
    e.next('.multi-BTO-select').removeClass('hide');
    //now hide all labels which has datachoices true but input unchecked
    e.closest('.item-wrap').find($('label[data-choices="true"] input:not(:checked)')).closest('label').next('.multi-BTO-select').addClass('hide');
    //if checkbox is unchecked
    if (e.find('input[type="checkbox"]').length && !e.find('input[type="checkbox"]').is(':checked')) {
      e.next('.multi-BTO-select').addClass('hide');
    }
  } else {
    $('label[data-choices="true"] input:not(:checked)').closest('label').next('.multi-BTO-select').addClass('hide');
  }
}

function onLoad_check_choices() { //check if onload the choice item is checked then update the sectionitemoptionid of the input
  $('label[data-choices="true"]').each(function() {
    if ($(this).find('input:checked').length) {
      update_sectionitemoptionid($(this).next('.multi-BTO-select'));
    }
  });
}

/////////////////////////Click Events////////////////

$('body').on('mousedown', 'label', function() { // finding value of previous checked radio box
  if (!$(this).find('input').hasClass('ignore')) { //ignore toggle price checboxes clicks
    if ($(this).find('input[type="radio"]').length > 0) {
      prevItem = $(this).find('input[type="radio"]').closest('.item-wrap').find('input[type="radio"]:checked');
      prevVal = prevItem.data('price');
      prevItemLabel = prevItem.closest('label');
      if (prevItemLabel.hasClass('label-with-spinner')) {
        if (prevItemLabel.prev('.spinner-input').find('input').val() != "") {
          prevVal = prevVal * prevItemLabel.prev('.spinner-input').find('input').val();
        }
      }
    }
  }
});

$('body').on('change', 'label', function() { //on radio or checkbox click
  if (!$(this).find('input').hasClass('ignore')) { //ignore toggle price checboxes clicks
   
    $('input').removeClass('last-clicked');
    $(this).find('input').addClass('last-clicked');

    check_spinner($(this));
    check_Total($(this));
    show_Other_Relative_Price($(this).find('input'));
    calculate_Relative_Price($(this));
    calculate_Date();
    check_choices($(this));
    check_filters($(this));

    if (prevItemLabel != 'undefined' || prevItemLabel.text() != '') { //proceed only if previous item exists in the seciton
      //this will execute when the item which has filter is unchecked
      if ($(this).find('input').attr('type') == 'radio') {
        check_prev_filters(prevItemLabel);
        check_filters_for_each();
      }
    }
    if ($(this).find('input').attr('type') == 'checkbox' && !$(this).find('input').is(':checked')) { //when checkbox is unchecked find and remove filter
      check_prev_filters($(this));
      check_filters_for_each();
      hide_checkbox_spinner($(this));
    }
  }

});
//this moves spinner beneath item
$('body').on('click', '.tabs', function() {
  move_spinner_beneath_item();
});

// on changing multi choice dropdown update the sectionitemoption id of the BTO item
$('body').on('change', '.multi-BTO-select', function() {
  update_sectionitemoptionid($(this));
});