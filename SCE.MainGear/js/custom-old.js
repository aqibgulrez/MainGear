
var initial = 4;
$('#addSteps').click(function(){
	var number = parseInt($(this).attr('data-value'));
	var data = new Array(number);
	$(data).each(function(i, v){
		initial = initial + 1;
		var html = '<div class="form-group"><label for="middle-name" class="control-label col-md-3 col-sm-3 col-xs-12">Step '+ initial +'</label><div class="col-md-6 col-sm-6 col-xs-12"><input id="middle-name" class="form-control col-md-7 col-xs-12" type="text" name="middle-name" data-parsley-id="9"></div></div>';
		$("#addMoreSteps").append(html);
	});
})




function addText(inputID, addAreaID){
	var dataAdd = $("#" + inputID).val();
	$("#" + addAreaID).append("<div class='new-subheading'><h2>" + dataAdd + "</h2><button class=\"btn btn-default\">Add BTO Item</button></div>");

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
$("#addNewItem").click(function(){
	//alert(cloneData());

	// data to add
	var cloneData = '<tr class="bto-item" id="cloner"><td><div class="checkbox"><label><input type="radio" class="flat" name="defaultItem"><span class="radio-styled"></span></label></div></td><td><select style="width: 100%;"><option>Item one</option><option>Item two</option></select></td><td><select style="width: 100%;"><option>Item one</option><option>Item two</option></select></td><td class="inline-checkboxes"><div class="checkbox"><label><input type="radio" class="flat" checked="checked" name="checkbox'+ radioName +'"><span class="radio-styled"></span> Radio</label></div><div class="checkbox"><label><input type="radio" class="flat" name="checkbox'+ radioName +'"><span class="radio-styled"></span> Checkbox</label></div></td></tr>';

	$("#resultCloned").append(cloneData);
	$('select').select2();
	$.getScript("../vendors/iCheck/icheck.min.js");
	radioName += 1;
})