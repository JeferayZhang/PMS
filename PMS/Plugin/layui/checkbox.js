// JavaScript Document
(function($){
	 $.fn.extend({		
		vjcheckbox:function(style){
		  //alert($(this).attr("id"));
		  var id=$(this).attr("id");		 
		  var checkid='#check_'+id+'_l'; 
		  if (style==1){  checkid='#'+$(this).attr("id");}	
		  if($('#check_'+id).is(":checked")){  $(checkid).addClass('checked');}else{ $(checkid).removeClass('checked');}
		 		 
			$(this).click( function() {	
				if($('#check_'+id).is(":checked")){     
					$(checkid).removeClass('checked');$('#check_'+id).prop("checked",false);
				}
				else{     
					$(checkid).addClass('checked');$('#check_'+id).prop("checked",true); 
				} 
			});
		}
	 });		
})(jQuery);

