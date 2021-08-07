
var initial = 4;
$('#addSteps').click(function(){
	var number = parseInt($(this).attr('data-value'));
	var data = new Array(number);
	$(data).each(function(i, v){
		initial = initial + 1;
		var html = '<div class="form-group"><label for="middle-name" class="control-label col-xs-2">Step '+ initial +'</label><div class="col-xs-8"><input id="middle-name" class="form-control col-md-7 col-xs-12" type="text" name="middle-name" data-parsley-id="9"></div></div>';
		$("#addMoreSteps").append(html);
	});
})




function addText(inputID, addAreaID){
	var dataAdd = $("#" + inputID).val();
	var newData = '<div class="data"><div class="table-heading"><h4>'+ dataAdd +'</h4><button type="button" class="btn btn-default btn-table-bto" onclick="modalCustom(\'BTOModal\')">Add BTO Item</button></div></div>';
	$("#" + addAreaID).append(newData);

}


//custom modal box
function modalCustom(modal){
	$("#" + modal).fadeIn(400);
	if ($("#" + modal).css('display')) {
		$("body").addClass("nav-md modal-open")
	}
}
function modalCustomClose(modal){ //custom modal box close
	$("#" + modal).fadeOut(400);
	$("body").removeClass("modal-open")
}




//creating new item to add in BTO item popup
function cloneData(){
	var cloned = $("#cloner").html();
	return cloned;
}
cloneData();
var radioName = 4;
$("#addNewItem").click(function () {
    //alert(cloneData());
    alert('abcd');
    // data to add
    var cloneData = '<div class="col-sm-4"><div class="bto-item"><div class="col-xs-12"><div class="checkbox"><label><input type="radio" class="flat" name="defaultItem"><span class="radio-styled"></span> Default</label></div></div><div class="col-xs-12"><table style="width: 100%"><tr><td style="width: 30%">BTO Item</td><td style="width: 70%"><select style="width: 100%;"><option>Item one</option><option>Item two</option></select></td></tr></table></div><div class="col-xs-12"><table style="width: 100%"><tr><td style="width: 30%">Wholesale</td><td style="width: 70%"><select style="width: 100%;"><option>Item one</option><option>Item two</option></select></td></tr></table></div><div class="col-xs-12"><table style="width: 100%"><tr><td style="width: 30%">Display Mode</td><td style="width: 70%" class="inline-checkboxes"><div class="checkbox"><label><input type="radio" class="flat" checked="checked" name="checkbox' + radioName + '"><span class="radio-styled"></span> Radio</label></div><div class="checkbox"><label><input type="radio" class="flat" name="checkbox' + radioName + '"><span class="radio-styled"></span> Checkbox</label></div></td></tr></table></div><div class="col-xs-12"><table style="width: 100%"><tr><td style="width: 30%">Processing Time</td><td style="width: 70%" class="inline-checkboxes"><div><input type="number" name="" placeholder="1" class="form-control" min="0"></div></td></tr></table></div><div class="col-xs-12"><table style="width: 100%"><tr><td style="width: 30%">Recommendation</td><td style="width: 70%" class="inline-checkboxes"><select style="width: 100%;"><option>None</option><option>GOOD</option><option>BETTER</option><option>BEST</option></select></td></tr></table></div><div class="col-xs-12 quantity-check-outer"><table style="width: 100%"><tr><td style="width: 30%">Quantity</td><td style="width: 70%" class="inline-checkboxes"><div class="checkbox inline-block"><label><input type="checkbox" class=" quantity-check" name="checkbox"><span class="radio-styled"></span></label></div></td></tr><tr><td style="width: 30%">Quantity Required</td><td style="width: 70%" class="inline-checkboxes"><div class="checkbox inline-block"><label><input type="checkbox" class="" name="required-check"><span class="radio-styled"></span></label></div></td></tr><tr><td style="width: 30%">Quantity Increment</td><td style="width: 70%" class="inline-checkboxes"><div class="checkbox inline-block"><label><input type="number" name="" class="quantity-check-input" placeholder="1"></label></div></td></tr></table></div></div></div>';

    $("#resultCloned").append(cloneData);
    $('select').select2();
    $(".spinner").spinner({ step: 2, min: 0 });
    radioName += 1;
});


$("#addNewCustomItem").click(function () {
    //alert(cloneData());

    // data to add
    var cloneData = '<div class="col-sm-4"><div class="bto-item"><div class="col-xs-12"><div class="checkbox"><label><input type="radio" name="defaultItem"><span class="radio-styled"></span> Default</label></div></div><div class="col-xs-12"><table style="width: 100%"><tr><td style="width: 30%">Label</td><td style="width: 70%"><input type="text" name="" class="form-control"></td></tr></table></div><div class="col-xs-12"><table style="width: 100%"><tr><td style="width: 30%">Required</td><td style="width: 70%" class=""><div class="checkbox inline-block"><label><input type="checkbox" name="checkbox"><span class="radio-styled"></span></label></div></td></tr></table></div></div></div>';

    $("#resultCustomCloned").append(cloneData);
    $('select').select2();
    $(".spinner").spinner({ step: 2, min: 0 });
    radioName += 1;
});





