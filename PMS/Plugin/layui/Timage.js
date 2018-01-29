// JavaScript Document
function resetImg(classname){
	var imagestr='';
	var width,height;
	var box_w,box_h;
	var tempname="."+classname;
	var imgarr=$(tempname);	
	$(("."+classname)).each(function(){
		box_w=$(this).width();
		box_h=$(this).height();	
		
		width=$(this).children("img").width();
		height=$(this).children("img").height();
		if((box_w/box_h)>(width/height)){
			//$(this).children("img").attr("width",(width*box_h/box_w)+"px");
			$(this).children("img").width(width*box_h/height);
			$(this).children("img").height(box_h);
			//$(this).children("img").css("padding-top",box_h+"px");
		}
		else{
			$(this).children("img").width(box_w);
			$(this).children("img").height(height*box_w/width);	
			$(this).children("img").css("margin-top",(box_h-box_w*height/width-1)/2+"px");
		}	
		$(this).children("img").css({"display":"block"});
	});	
};
$(function(){resetImg("Timage_auto");});