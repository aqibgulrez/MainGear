  $(document).ready(function() {
    ShowStep('#Step3');
  });

  function ShowStep(id) {
    $("#Step1").hide();
    $("#Step2").hide();
    $("#Step3").hide();
    $("#Step4").hide();
    $("#Step5").hide();
    $("#Step6").hide();
    $("#Step7").hide();
    $(id).show();
  }

  $(function() {


    //on scroll down fix the sub total area
    $(window).scroll(function() {
      var matchWidthGrid = $(".wrapper .right-container").parent().width(); //height of the parent  
      var matchWidth = $(".container").width();
      matchWidth = (25 / 100 * matchWidth) - 35;
      // console.log(matchWidth)
      if ($(window).width() >= 993) {
        if ($(window).scrollTop() > 400) {
          $(".right-container").addClass("fix-on-right");
          $('.fix-on-right').css('width', matchWidth);
        } else {
          $(".right-container").removeClass("fix-on-right");
          $('.right-container').css('width', '100%');
        }
      } else {
        $(".right-container").removeClass("fix-on-right");
        $('.right-container').css('width', '100%');
      }

    });

  });

  //lightbox popup
  function showlightbox(id) {
    $("#" + id).fadeIn(250);
    $
    return false;
  }
  //lightbox close
  function closepopup(id) {
    $("#" + id).fadeOut(250);
  }



  //lightbox on cursor position
  function lightboxOnCursor(caller, id) {
    $(caller).on('mousemove', function(e) {
      //console.log("e.pageX: " + e.pageX + ", e.pageY: " + e.pageY); 
      $("#" + id).addClass('popup-set-on-cursor').find('.popup-inner').css({ 'left': 1 + e.pageX, 'top': 1 + e.pageY });
      $(caller).addClass('current-image-popup');
      $("#" + id).addClass('current-image-popup');
    });
    $(".current-image-popup").mouseenter(function() {
      $("#" + id).show();
    });
    $(".current-image-popup").mouseleave(function() {
      $("#" + id).hide(200);
    });
  };




  //sorting sections
  function sortingSections(order) {

    //adding price total values
    $(".build-box").each(function(i, v) {
      var totalThis = 0;
      $(this).find('.add-on').each(function(i, v) {
        totalThis = totalThis + parseFloat($(this).attr('data-price'));
      })
      $(this).attr('data-sortprice', totalThis);
    });

    //sorting through price total values
    $('.get-price-sort').filter(function() {
      $(this).find(".build-box").sort(sort_price).appendTo(this);

      function sort_price(a, b) {
        if (order == "asc") {
          return ($(b).data('sortprice')) < ($(a).data('sortprice')) ? 1 : -1;
        } else if (order == "desc") {
          return ($(b).data('sortprice')) > ($(a).data('sortprice')) ? 1 : -1;
        }
      }
    });

  }