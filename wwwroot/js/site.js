$(document).ready(function(e){
	//Get state from url
	var state = getUrlParameter('p');

	//Search dropdown functionality
	if (state) {
		$('.search-panel span#search_concept').text(state);
		$('.input-group #search_param').val(state);
	}
		
    $('.search-panel .dropdown-menu').find('a').click(function(e) {
		e.preventDefault(); // Stop the form from submitting
		var param = $(this).attr("href").replace("#","");
		var concept = $(this).text();
		$('.search-panel span#search_concept').text(concept);
		$('.input-group #search_param').val(param);
		setFocusToSearchBar();
	});

	//Stop search if no state has been selected
	$("#searchSubmit").click(function (e) {
		var stateSelection = $("#search_param");
		if (stateSelection.val() == "") {
			e.preventDefault(); // Stop the form from submitting
			$("#searchFormAlert").fadeIn();
			return false;
		}
		else {
			$("#searchFormAlert").fadeOut();
			return true;
        }
	});

	//Stop ENTER from submitting createPlanForm
	$('#createPlanForm').on('keyup keypress', function(e) 
	{
		var keyCode = e.keyCode || e.which;
		if (keyCode === 13) 
		{
		  e.preventDefault();
		  return false;
		}
	});

	//Stop ENTER from submitting editPlanForm
	$('#editPlanForm').on('keyup keypress', function(e) 
	{
		var keyCode = e.keyCode || e.which;
		if (keyCode === 13) 
		{
			e.preventDefault();
			return false;
		}
	});

	//Tooltip functionality
	$('[data-toggle="tooltip"]').tooltip(); 
});

//Set focus to search bar after state is selected
setFocusToSearchBar = function getFocus() {
	document.getElementById("reggiesearchfield").focus();
  };

//get url parameter
getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};