/*=================================

PranksterApp Javascript Core

===================================*/

$(function(){
    "use strict";

    /*================================================================
      01. Preloader --- deprecated
    =================================================================*/

    /*
    $( document ).ready(function() {
        setTimeout(()=>{
            $('.preloader').fadeOut();
        }, 250)
    });
    */
    /*

    $(document).ready(function () {
        $(".transitions-only-after-load").each(function (index, element) {
            setTimeout(function () { $(element).removeClass("transitions-only-after-load") }, 10);
        });
    });
    */
    

    /*================================================================
      01. grid
    =================================================================*/

    let vline = document.querySelectorAll('.dz-vline');
        vline.forEach(function(cur){
            cur.style.flex = `0 0 calc(calc( ${cur.getAttribute('data-display')} / 12 ) * 100%)`;
            cur.style.width = `calc(calc( ${cur.getAttribute('data-display')} / 12 ) * 100%)`;
        });

    /*================================================================
      01. background image
    =================================================================*/
    document.querySelectorAll('[data-img]').forEach(function(cur){
        cur.style.backgroundImage = 'url(' + cur.getAttribute('data-img') + ')';
        cur.style.backgroundSize = 'cover';
        cur.style.backgroundRepeat = 'no-repeat';
        cur.style.backgroundPosition = 'center';
    });

    /*================================================================
      01. go back function
    =================================================================*/
    let goBack = document.querySelector('.goBack');
       let goBackPage = ()=>{history.go(-1)}
       goBack ? goBack.addEventListener('click', goBackPage) : null

    /*================================================================
      01. home notification popup
    =================================================================*/
    /*
    $('.dz-bell').on('click',function(){
      $('.dz-nav').toggleClass('dz-show');
    });
    */
    /*================================================================
      01. svg image change
    =================================================================*/

    const convertImages = (query, callback) => {
        const images = document.querySelectorAll(query);
        
            images.forEach(image => {
            fetch(image.src)
            .then(res => res.text())
            .then(data => {
                const parser = new DOMParser();
                const svg = parser.parseFromString(data, 'image/svg+xml').querySelector('svg');
        
                if (image.id) svg.id = image.id;
                if (image.className) svg.classList = image.classList;
        
                image.parentNode.replaceChild(svg, image);
            })
            .then(callback)
            .catch(error =>error);
            });
        };
        convertImages('.svg');
      /*================================================================
          01. menu toggle
      =================================================================*/
      $('.toggle-btn').on('click',function(){
            $('.m-menu').addClass('show');
            $('.m-menu__overlay').addClass('show');
        });
        $('.m-menu__close').on('click',function(){
            $('.m-menu').removeClass('show');
            $('.m-menu__overlay').removeClass('show');
        });

        /*================================================================
      01. swiper slider
    =================================================================*/
      // Home slider 1
    new Swiper('.dz-near',{
        slidesPerView: 2,
        spaceBetween: 10,
        observer: true,
      observeParents: true,
      loop: true,
      centeredSlides: false,
      breakpoints: {
          1920: {
            slidesPerView: 2,
            initialSlide: 1,
          },
          1450: {
            slidesPerView: 2,
            initialSlide: 1,
          },
          767: {
              slidesPerView: 2,
          },
          576: {
              slidesPerView: 2,
          },
          320: {
              slidesPerView: 2,
          }
      }
      
    });

});