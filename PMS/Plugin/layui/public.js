var vjong_uijs=function(){
	this.writer='vjliujinchen';
	this.status={};
	var temp$=this;
	
	/*---------------------------------*/
	/*点击显示隐藏功能*/
	/*---------------------------------*/
	this.popshow=function(fdomid1,fdomid2,fstatus1){
		var tempfun=function(domid1,domid2,status1){
			if (typeof(temp$.status[domid1])!='undefined'){
				status1=temp$.status[domid1];
			}
			if (status1){
				temp$.status[domid1]=false;
				$('#'+domid2).show();
			}else{
				temp$.status[domid1]=true;
				$('#'+domid2).hide();
			}
		}
		tempfun(fdomid1,fdomid2,fstatus1);
		$('#'+fdomid1).click(function(){
			tempfun(fdomid1,fdomid2,fstatus1);
		});				
	};
	
	/*---------------------------------*/
	/*点击交替动作*/
	/*---------------------------------*/
	this.clickdo=function(domid,doeven1,doeven2,status1){				
		$('#'+domid).click(function(){
		console.log(temp$.status[domid]);
			if (typeof(temp$.status[domid])!='undefined'){
				status1=temp$.status[domid];
			}
			if (status1){
				temp$.status[domid]=false;
				eval(doeven2);		
			}else{
				temp$.status[domid]=true;
				eval(doeven1);
			}
		});				
	}
};
var vjui=new vjong_uijs();