$(function(){  //skeleton for jquery

 //////////////////////////////////setting up variables////////////////////////////////
var home_El=$("#home_el")
var tracking_El=$("#tracking_el")
var contact_El=$("#contact_el")
var visit_Btn = $(".visit-btn")
var course_Col = $(".sol-col")
var center_Layer = $(".layer")
window.sr = ScrollReveal();

//////////////////////////////////////////Fading effect/////////////////
$(".text-box").css('display', 'none');
$(".text-box").fadeIn(1200);
$(".nav-links").css('display', 'none');
$(".nav-links").fadeIn(1200);
$(".yuvologo").css('display', 'none');
$(".yuvologo").fadeIn(1200);
sr.reveal(".solutions")
sr.reveal(".center")
sr.reveal(".contactus")
sr.reveal(".footer")








/////////////////////////setting underline effect on hover for list items //////////////////////////////
home_El.mouseenter(function(){
    $(this).css({
        'text-decoration':'underline',
        'text-underline-offset':'3px',
        'background':'#f44336',
        'size':'3px'
    } )})

 home_El.mouseleave(function(){
        $(this).css({
            'text-decoration':'none',
            'background':'none'
 } )})

 tracking_El.mouseenter(function(){
    $(this).css({
        'text-decoration':'underline',
        'text-underline-offset':'3px',
        'background':'#f44336'} )})

        tracking_El.mouseleave(function(){
        $(this).css({
            'text-decoration':'none',
            'background':'none'
 } )})
//  blog_El.mouseenter(function(){
//     $(this).css({
//         'text-decoration':'underline',
//         'text-underline-offset':'3px',
//         'background':'#f44336'} )})

// blog_El.mouseleave(function(){
//         $(this).css({
//             'text-decoration':'none',
//             'background':'none'
//  } )})

//  about_El.mouseenter(function(){
//     $(this).css({
//         'text-decoration':'underline',
//         'text-underline-offset':'3px',
//         'background':'#f44336'} )})

//         about_El.mouseleave(function(){
//         $(this).css({
//             'text-decoration':'none',
//             'background':'none'
//  } )})  

 contact_El.mouseenter(function(){
    $(this).css({
        'text-decoration':'underline',
        'text-underline-offset':'3px',
        'background':'#f44336'} )})

contact_El.mouseleave(function(){
        $(this).css({
            'text-decoration':'none',
            'background':'none'
 } )})         
/////////////////////////setting underline effect on hover for list items //////////////////////////////

visit_Btn.mouseenter(function(){
    $(this).stop().css({
        'border': '1px solid #f44336 ',
        'background':'#f44336',
        'transition':'0.5s'
    })
})
visit_Btn.mouseleave(function(){
    $(this).stop().css({
        'border': '1px solid #fff ',
        'background':'transparent'
        
    })
})
/////////////////////////setting underline effect on hover for list items //////////////////////////////



//////////////////////////////////////////////Solution Section//////////////////////////////////////////////////////

course_Col.mouseenter(function(){
    $(this).stop().css({
        'box-shadow':'0 0 20px 0px rgba(0,0,0,0.2)',
        transition:'0.5s'
})
})
course_Col.mouseleave(function(){
    $(this).stop().css({
        'box-shadow': 'none',
        transition:'0.5s'

    })
})

//////////////////////////////////////////////Centers Section//////////////////////////////////////////////////////

center_Layer.mouseenter(function(){
    $(this).css({
        'background':'rgba(226,0,0,0.7)',
        'transition':'0.5s'
    })
})

center_Layer.mouseleave(function(){
    $(this).css({
        'background':'transparent',
        'transition':'0.5s'
    })
})




}); 