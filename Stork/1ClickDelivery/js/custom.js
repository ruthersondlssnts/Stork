/*global jQuery:false */
(function ($) {


    var navActive = function (section) {

        var $el = $('.collapse > ul');
        $el.find('li').removeClass('active');
        $el.each(function () {
            $(this).find('a[data-nav-section="' + section + '"]').closest('li').addClass('active');
            $(this).find('a[data-nav-sections="' + section + '"]').closest('li').addClass('active');
        });

    };
  

        var $section = $('section[data-section]');

        $section.waypoint(function (direction) {

            if (direction === 'down') {
                navActive($(this.element).data('section'));
            }
        }, {
                offset: '150px'
            });

        $section.waypoint(function (direction) {
            if (direction === 'up') {
                navActive($(this.element).data('section'));

            }
        }, {
                offset: function () { return -$(this.element).height() + 155; }
            });


   
    $('.panel-collapse').on('shown.bs.collapse', function () {
        $(this).parent().find('.fa-plus').removeClass('fa-plus').addClass('fa-minus');
    }).on('hidden.bs.collapse', function () {
        $(this).parent().find('.fa-minus').removeClass('fa-minus').addClass('fa-plus');
    });
  var wow = new WOW({
    boxClass: 'wow', // animated element css class (default is wow)
    animateClass: 'animated', // animation css class (default is animated)
    offset: 0 // distance to the element when triggering the animation (default is 0)

  });
    wow.init();


  //jQuery to collapse the navbar on scroll
  $(window).scroll(function() {
    if ($(".navbar").offset().top > 50) {
      $(".navbar-fixed-top").addClass("top-nav-collapse");
      $(".top-area").addClass("top-padding");
      $(".navbar-brand").addClass("reduce");

      $(".navbar-custom ul.nav ul.dropdown-menu").css("margin-top", "11px");

    } else {
      $(".navbar-fixed-top").removeClass("top-nav-collapse");
      $(".top-area").removeClass("top-padding");
      $(".navbar-brand").removeClass("reduce");

      $(".navbar-custom ul.nav ul.dropdown-menu").css("margin-top", "16px");

    }
  });

	var navMain = $(".navbar-collapse"); 
	navMain.on("click", "a:not([data-toggle])", null, function () {
	   navMain.collapse('hide');
	});

  //scroll to top
  $(window).scroll(function() {
    if ($(this).scrollTop() > 100) {
      $('.scrollup').fadeIn();
    } else {
      $('.scrollup').fadeOut();
    }
  });
  $('.scrollup').click(function() {
    $("html, body").animate({
      scrollTop: 0
    }, 1000);
    return false;
  });
  
    $('.hero__scroll-button').click(function () {
        $("html, body").animate({ scrollTop: $('#metro').offset().top }, 1500);
        return false;
    });




  //jQuery for page scrolling feature - requires jQuery Easing plugin
    $(function () {
        $('.navbar-nav li a').bind('click', function (event) {
            var section = $(this).data('nav-section');
            if (section == "metro") {
                if ($('[data-section="' + section + '"]').length) {
                    $('html, body').stop().animate({
                        scrollTop: $('[data-section="' + section + '"]').offset().top - 250
                    }, 1500, 'easeInOutExpo');

                    event.preventDefault();
                }
            }
            else {
                if ($('[data-section="' + section + '"]').length) {
                    $('html, body').stop().animate({
                        scrollTop: $('[data-section="' + section + '"]').offset().top - 55
                    }, 1500, 'easeInOutExpo');

                    event.preventDefault();
                }
            }
           
        });
        $('.page-scroll a').bind('click', function (event) {
            var section = $(this).data('nav-section');
            $('html, body').stop().animate({
                scrollTop: $('[data-section="' + section + '"]').offset().top
            }, 1500, 'easeInOutExpo');
            event.preventDefault();
        });

       
    });

  //owl carousel
    var owl = $('.owl-carousel');
    owl.owlCarousel({
        items: 1,
        loop: true,
        autoplay: 5000
    });
  //nivo lightbox


  
    
jQuery('.appear').appear();
  jQuery(".appear").on("appear", function(data) {
      var id = $(this).attr("id");
    jQuery('.nav li').removeClass('active');
    jQuery(".nav a[href='#" + id + "']").parent().addClass("active");
    });




  //parallax
  if ($('.parallax').length) {
    $(window).stellar({
      responsive: true,
      scrollProperty: 'scroll',
      parallaxElements: false,
      horizontalScrolling: false,
      horizontalOffset: 0,
      verticalOffset: 0
    });

  }


  (function($, window, document, undefined) {

    var gridContainer = $('#grid-container'),
      filtersContainer = $('#filters-container');

    // init cubeportfolio
    gridContainer.cubeportfolio({

      defaultFilter: '*',

      animationType: 'sequentially',

      gapHorizontal: 50,

      gapVertical: 40,

      gridAdjustment: 'responsive',

      caption: 'fadeIn',

      displayType: 'lazyLoading',

      displayTypeSpeed: 100,

      // lightbox
      lightboxDelegate: '.cbp-lightbox',
      lightboxGallery: true,
      lightboxTitleSrc: 'data-title',
      lightboxShowCounter: true,

      // singlePage popup
      singlePageDelegate: '.cbp-singlePage',
      singlePageDeeplinking: true,
      singlePageStickyNavigation: true,
      singlePageShowCounter: true,
      singlePageCallback: function(url, element) {

        // to update singlePage content use the following method: this.updateSinglePage(yourContent)
        var t = this;

        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'html',
            timeout: 5000
          })
          .done(function(result) {
            t.updateSinglePage(result);
          })
          .fail(function() {
            t.updateSinglePage("Error! Please refresh the page!");
          });

      },

      // singlePageInline
      singlePageInlineDelegate: '.cbp-singlePageInline',
      singlePageInlinePosition: 'above',
      singlePageInlineShowCounter: true,
      singlePageInlineInFocus: true,
      singlePageInlineCallback: function(url, element) {
        // to update singlePageInline content use the following method: this.updateSinglePageInline(yourContent)
      }
    });

    // add listener for filters click
    filtersContainer.on('click', '.cbp-filter-item', function(e) {

      var me = $(this),
        wrap;

      // get cubeportfolio data and check if is still animating (reposition) the items.
      if (!$.data(gridContainer[0], 'cubeportfolio').isAnimating) {

        if (filtersContainer.hasClass('cbp-l-filters-dropdown')) {
          wrap = $('.cbp-l-filters-dropdownWrap');

          wrap.find('.cbp-filter-item').removeClass('cbp-filter-item-active');

          wrap.find('.cbp-l-filters-dropdownHeader').text(me.text());

          me.addClass('cbp-filter-item-active');
        } else {
          me.addClass('cbp-filter-item-active').siblings().removeClass('cbp-filter-item-active');
        }

      }

      // filter the items
      gridContainer.cubeportfolio('filter', me.data('filter'), function() {});

    });

    // activate counter for filters
    gridContainer.cubeportfolio('showCounter', filtersContainer.find('.cbp-filter-item'));

  })(jQuery, window, document);


})(jQuery);
$(window).load(function() {
  $(".loader").delay(100).fadeOut();
  $("#page-loader").delay(100).fadeOut("fast");
});
