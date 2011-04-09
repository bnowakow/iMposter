RIAStats={};RIAStats.version="4";RIAStats.init=function(){RIAStats.createCookie('riastats_cookieTest','test',2);var cookieTest=RIAStats.readCookie('riastats_cookieTest');if(cookieTest!='test'){RIAStats.createCookie('riastats_lock','1',1);return;}
RIAStats.eraseCookie('riastats_cookieTest');var oldTime=RIAStats.readCookie('riastats_lastDate');var newTime=(new Date()).getTime();RIAStats.createCookie('riastats_lastDate',newTime);if(oldTime==null){oldTime=newTime;}
var timeDifference=newTime-oldTime;var daysDifference=Math.round(timeDifference/1000/60/60/24);var pastSwfVer=RIAStats.readCookie('riastatsV'+RIAStats.version+'_pastFlashPlayerVersion');var pastAgVer=RIAStats.readCookie('riastatsV'+RIAStats.version+'_pastSilverLightVersion');var pastJavaVer=RIAStats.readCookie('riastatsV'+RIAStats.version+'_pastJavaVersion');var pastJhtmlVer=RIAStats.readCookie('riastatsV'+RIAStats.version+'_pastHTMLVersion');var swfVersion=RIAStats.GetSwfVer();var agVersion=RIAStats.getSilverlightVersion();var javaVersions=RIAStats.DeployJava.getJREs();var htmlVersion=RIAStats.getHTMLVersion();if(javaVersions.length>0)
javaVersion=javaVersions[0];else
javaVersion="";var updateRequired=false;var riaStatsHost=(("https:"==document.location.protocol)?"https://":"http://");var req=riaStatsHost+"riastats.com/reportBrowserPlugins/?pastHtmlVersion="+pastJhtmlVer+"&htmlVersion="+htmlVersion+"&pastJavaVersion="+pastJavaVer+"&javaVersion="+javaVersion+"&swfVersion="+swfVersion+"&pastSwfVersion="+pastSwfVer+"&agVersion="+agVersion+"&pastAgVersion="+pastAgVer+"&siteID="+riaStats_siteID+"&days="+daysDifference;bObj=new RIAStats.JSONscriptRequest(req);bObj.buildScriptTag();bObj.addScriptTag();RIAStats.createCookie('riastatsV'+RIAStats.version+'_pastFlashPlayerVersion',swfVersion,365);RIAStats.createCookie('riastatsV'+RIAStats.version+'_pastSilverLightVersion',agVersion,365);RIAStats.createCookie('riastatsV'+RIAStats.version+'_pastJavaVersion',javaVersion,365);RIAStats.createCookie('riastatsV'+RIAStats.version+'_pastHTMLVersion',htmlVersion,365);RIAStats.createCookie('riastats_lock','1',1);}
RIAStats.getHTMLVersion=function(){if(RIAStats.supportsHTML5Canvas()&&RIAStats.supports_video()&&(RIAStats.supports_webm_video()||RIAStats.supports_h264_baseline_video()||RIAStats.supports_ogg_theora_video()))
return 5.01;if(RIAStats.supportsHTML5Canvas())
return 5.0;return 4.0;}
RIAStats.supports_video=function(){return!!document.createElement('video').canPlayType;}
RIAStats.supports_webm_video=function(){var v=document.createElement("video");return v.canPlayType('video/webm; codecs="vp8, vorbis"');}
RIAStats.supports_h264_baseline_video=function(){var v=document.createElement("video");return v.canPlayType('video/mp4; codecs="avc1.42E01E, mp4a.40.2"');}
RIAStats.supports_ogg_theora_video=function(){var v=document.createElement("video");return v.canPlayType('video/ogg; codecs="theora, vorbis"');}
RIAStats.supportsHTML5Canvas=function(){return!!document.createElement('canvas').getContext;}
RIAStats.supportsHTML4IFrame=function(){return!!document.createElement('iframe').src!=undefined;}
RIAStats.getSilverlightVersion=function(){var version='-1';var container=null;try{var control=null;if(window.ActiveXObject){control=new ActiveXObject('AgControl.AgControl');}
else{if(navigator.plugins['Silverlight Plug-In']){container=document.createElement('div');document.body.appendChild(container);container.innerHTML='<embed type="application/x-silverlight" src="data:," />';control=container.childNodes[0];}}
if(control){if(control.isVersionSupported('4.0')){version='4.0';}
else if(control.isVersionSupported('3.0')){version='3.0';}
else if(control.isVersionSupported('2.0')){version='2.0';}
else if(control.isVersionSupported('1.0')){version='1.0';}}}
catch(e){}
if(container){document.body.removeChild(container);}
return version;}
RIAStats.isNull=function(val){return(val==null);}
RIAStats.createCookie=function(name,value,days){var expires="";if(days){var date=new Date();date.setTime(date.getTime()+(days*24*60*60*1000));expires="; expires="+date.toGMTString();}
document.cookie=name+"="+value+expires+"; path=/";}
RIAStats.readCookie=function(name){var nameEQ=name+"=";var ca=document.cookie.split(';');for(var i=0;i<ca.length;i++){var c=ca[i];while(c.charAt(0)==' ')c=c.substring(1,c.length);if(c.indexOf(nameEQ)==0)return c.substring(nameEQ.length,c.length);}
return null;}
RIAStats.eraseCookie=function(name){RIAStats.createCookie(name,"",-1);}
RIAStats.JSONscriptRequest=function(fullUrl){this.fullUrl=fullUrl;this.noCacheIE='&noCacheIE='+(new Date()).getTime();this.headLoc=document.getElementsByTagName("head").item(0);this.scriptId='JscriptId'+RIAStats.JSONscriptRequest.scriptCounter++;}
RIAStats.JSONscriptRequest.scriptCounter=1;RIAStats.JSONscriptRequest.prototype.buildScriptTag=function(){this.scriptObj=document.createElement("script");this.scriptObj.setAttribute("type","text/javascript");this.scriptObj.setAttribute("charset","utf-8");this.scriptObj.setAttribute("src",this.fullUrl+this.noCacheIE);this.scriptObj.setAttribute("id",this.scriptId);}
RIAStats.JSONscriptRequest.prototype.removeScriptTag=function(){this.headLoc.removeChild(this.scriptObj);}
RIAStats.JSONscriptRequest.prototype.addScriptTag=function(){this.headLoc.appendChild(this.scriptObj);}
RIAStats.isIE=(navigator.appVersion.indexOf("MSIE")!=-1)?true:false;RIAStats.isWin=(navigator.appVersion.toLowerCase().indexOf("win")!=-1)?true:false;RIAStats.isOpera=(navigator.userAgent.indexOf("Opera")!=-1)?true:false;RIAStats.ControlVersion=function()
{var version;var axo;var e;try{axo=new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7");version=axo.GetVariable("$version");}catch(e){}
if(!version)
{try{axo=new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6");version="WIN 6,0,21,0";axo.AllowScriptAccess="always";version=axo.GetVariable("$version");}catch(e){}}
if(!version)
{try{axo=new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");version=axo.GetVariable("$version");}catch(e){}}
if(!version)
{try{axo=new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");version="WIN 3,0,18,0";}catch(e){}}
if(!version)
{try{axo=new ActiveXObject("ShockwaveFlash.ShockwaveFlash");version="WIN 2,0,0,11";}catch(e){version=-1;}}
return version;}
RIAStats.GetSwfVer=function(){var flashVer=-1;if(navigator.plugins!=null&&navigator.plugins.length>0){if(navigator.plugins["Shockwave Flash 2.0"]||navigator.plugins["Shockwave Flash"]){var swVer2=navigator.plugins["Shockwave Flash 2.0"]?" 2.0":"";var flashDescription=navigator.plugins["Shockwave Flash"+swVer2].description;var descArray=flashDescription.split(" ");var tempArrayMajor=descArray[2].split(".");var versionMajor=tempArrayMajor[0];var versionMinor=tempArrayMajor[1];var versionRevision=descArray[3];if(versionRevision==""){versionRevision=descArray[4];}
if(versionRevision[0]=="d"){versionRevision=versionRevision.substring(1);}else if(versionRevision[0]=="r"){versionRevision=versionRevision.substring(1);if(versionRevision.indexOf("d")>0){versionRevision=versionRevision.substring(0,versionRevision.indexOf("d"));}}
var flashVer=versionMajor+"."+versionMinor+"."+versionRevision;}}
else if(navigator.userAgent.toLowerCase().indexOf("webtv/2.6")!=-1)flashVer=4;else if(navigator.userAgent.toLowerCase().indexOf("webtv/2.5")!=-1)flashVer=3;else if(navigator.userAgent.toLowerCase().indexOf("webtv")!=-1)flashVer=2;else if(RIAStats.isIE&&RIAStats.isWin&&!RIAStats.isOpera){flashVer=RIAStats.ControlVersion();}
return flashVer;}
RIAStats.DetectFlashVer=function(reqMajorVer,reqMinorVer,reqRevision)
{versionStr=RIAStats.GetSwfVer();if(versionStr==-1){return false;}else if(versionStr!=0){if(RIAStats.isIE&&RIAStats.isWin&&!RIAStats.isOpera){tempArray=versionStr.split(" ");tempString=tempArray[1];versionArray=tempString.split(",");}else{versionArray=versionStr.split(".");}
var versionMajor=versionArray[0];var versionMinor=versionArray[1];var versionRevision=versionArray[2];if(versionMajor>parseFloat(reqMajorVer)){return true;}else if(versionMajor==parseFloat(reqMajorVer)){if(versionMinor>parseFloat(reqMinorVer))
return true;else if(versionMinor==parseFloat(reqMinorVer)){if(versionRevision>=parseFloat(reqRevision))
return true;}}
return false;}}
RIAStats.AC_AddExtension=function(src,ext)
{if(src.indexOf('?')!=-1)
return src.replace(/\?/,ext+'?');else
return src+ext;}
RIAStats.AC_Generateobj=function(objAttrs,params,embedAttrs)
{var str='';if(RIAStats.isIE&&RIAStats.isWin&&!RIAStats.isOpera)
{str+='<object ';for(var i in objAttrs)
str+=i+'="'+objAttrs[i]+'" ';for(var i in params)
str+='><param name="'+i+'" value="'+params[i]+'" /> ';str+='></object>';}else{str+='<embed ';for(var i in embedAttrs)
str+=i+'="'+embedAttrs[i]+'" ';str+='> </embed>';}
document.write(str);}
RIAStats.AC_FL_RunContent=function(){var ret=RIAStats.AC_GetArgs
(arguments,".swf","movie","clsid:d27cdb6e-ae6d-11cf-96b8-444553540000","application/x-shockwave-flash");RIAStats.AC_Generateobj(ret.objAttrs,ret.params,ret.embedAttrs);}
RIAStats.AC_GetArgs=function(args,ext,srcParamName,classid,mimeType){var ret=new Object();ret.embedAttrs=new Object();ret.params=new Object();ret.objAttrs=new Object();for(var i=0;i<args.length;i=i+2){var currArg=args[i].toLowerCase();switch(currArg){case"classid":break;case"pluginspage":ret.embedAttrs[args[i]]=args[i+1];break;case"src":case"movie":args[i+1]=RIAStats.AC_AddExtension(args[i+1],ext);ret.embedAttrs["src"]=args[i+1];ret.params[srcParamName]=args[i+1];break;case"onafterupdate":case"onbeforeupdate":case"onblur":case"oncellchange":case"onclick":case"ondblClick":case"ondrag":case"ondragend":case"ondragenter":case"ondragleave":case"ondragover":case"ondrop":case"onfinish":case"onfocus":case"onhelp":case"onmousedown":case"onmouseup":case"onmouseover":case"onmousemove":case"onmouseout":case"onkeypress":case"onkeydown":case"onkeyup":case"onload":case"onlosecapture":case"onpropertychange":case"onreadystatechange":case"onrowsdelete":case"onrowenter":case"onrowexit":case"onrowsinserted":case"onstart":case"onscroll":case"onbeforeeditfocus":case"onactivate":case"onbeforedeactivate":case"ondeactivate":case"type":case"codebase":ret.objAttrs[args[i]]=args[i+1];break;case"id":case"width":case"height":case"align":case"vspace":case"hspace":case"class":case"title":case"accesskey":case"name":case"tabindex":ret.embedAttrs[args[i]]=ret.objAttrs[args[i]]=args[i+1];break;default:ret.embedAttrs[args[i]]=ret.params[args[i]]=args[i+1];}}
ret.objAttrs["classid"]=classid;if(mimeType)ret.embedAttrs["type"]=mimeType;return ret;}
RIAStats.addEvent=function(obj,evType,fn){if(obj.addEventListener){obj.addEventListener(evType,fn,false);return true;}else if(obj.attachEvent){var r=obj.attachEvent("on"+evType,fn);return r;}else{return false;}}
RIAStats.DeployJava={debug:null,myInterval:null,preInstallJREList:null,returnPage:null,brand:null,locale:null,installType:null,EAInstallEnabled:false,EarlyAccessURL:null,oldMimeType:'application/npruntime-scriptable-plugin;DeploymentToolkit',mimeType:'application/java-deployment-toolkit',getJREs:function(){var list=new Array();if(RIAStats.DeployJava.isPluginInstalled()){var plugin=RIAStats.DeployJava.getPlugin();var VMs=plugin.jvms;for(var i=0;i<VMs.getLength();i++){list[i]=VMs.get(i).version;}}else{var browser=RIAStats.DeployJava.getBrowser();if(browser=='MSIE'){if(RIAStats.DeployJava.testUsingActiveX('1.7.0')){list[0]='1.7.0';}else if(RIAStats.DeployJava.testUsingActiveX('1.6.0')){list[0]='1.6.0';}else if(RIAStats.DeployJava.testUsingActiveX('1.5.0')){list[0]='1.5.0';}else if(RIAStats.DeployJava.testUsingActiveX('1.4.2')){list[0]='1.4.2';}else if(RIAStats.DeployJava.testForMSVM()){list[0]='1.1';}}else if(browser=='Netscape Family'){RIAStats.DeployJava.getJPIVersionUsingMimeType();if(RIAStats.DeployJava.firefoxJavaVersion!=null){list[0]=RIAStats.DeployJava.firefoxJavaVersion;}else if(RIAStats.DeployJava.testUsingMimeTypes('1.7')){list[0]='1.7.0';}else if(RIAStats.DeployJava.testUsingMimeTypes('1.6')){list[0]='1.6.0';}else if(RIAStats.DeployJava.testUsingMimeTypes('1.5')){list[0]='1.5.0';}else if(RIAStats.DeployJava.testUsingMimeTypes('1.4.2')){list[0]='1.4.2';}else if(RIAStats.DeployJava.browserName2=='Safari'){if(RIAStats.DeployJava.testUsingPluginsArray('1.7.0')){list[0]='1.7.0';}else if(RIAStats.DeployJava.testUsingPluginsArray('1.6')){list[0]='1.6.0';}else if(RIAStats.DeployJava.testUsingPluginsArray('1.5')){list[0]='1.5.0';}else if(RIAStats.DeployJava.testUsingPluginsArray('1.4.2')){list[0]='1.4.2';}}}}
if(RIAStats.DeployJava.debug){for(var i=0;i<list.length;++i){alert('We claim to have detected Java SE '+list[i]);}}
return list;},getJPIVersionUsingMimeType:function(){for(var i=0;i<navigator.mimeTypes.length;++i){var s=navigator.mimeTypes[i].type;var m=s.match(/^application\/x-java-applet;jpi-version=(.*)$/);if(m!=null){RIAStats.DeployJava.firefoxJavaVersion=m[1];break;}}},isPluginInstalled:function(){var plugin=RIAStats.DeployJava.getPlugin();if(plugin&&plugin.jvms){return true;}else{return false;}},allowPlugin:function(){RIAStats.DeployJava.getBrowser();var ret=('Chrome'!=RIAStats.DeployJava.browserName2&&'Safari'!=RIAStats.DeployJava.browserName2&&'Opera'!=RIAStats.DeployJava.browserName2);return ret;},getPlugin:function(){RIAStats.DeployJava.refresh();var ret=null;if(RIAStats.DeployJava.allowPlugin()){ret=document.getElementById('deployJavaPlugin');}
return ret;},compareVersionToPattern:function(version,patternArray,familyMatch){var regex="^(\\d+)(?:\\.(\\d+)(?:\\.(\\d+)(?:_(\\d+))?)?)?$";var matchData=version.match(regex);if(matchData!=null){var index=0;var result=new Array();for(var i=1;i<matchData.length;++i){if((typeof matchData[i]=='string')&&(matchData[i]!=''))
{result[index]=matchData[i];index++;}}
var l=Math.min(result.length,patternArray.length);if(familyMatch){for(var i=0;i<l;++i){if(result[i]!=patternArray[i])return false;}
return true;}else{for(var i=0;i<l;++i){if(result[i]<patternArray[i]){return false;}else if(result[i]>patternArray[i]){return true;}}
return true;}}else{return false;}},getBrowser:function(){if(RIAStats.DeployJava.browserName==null){var browser=navigator.userAgent.toLowerCase();if(RIAStats.DeployJava.debug){alert('userAgent -> '+browser);}
if(browser.indexOf('msie')!=-1){RIAStats.DeployJava.browserName='MSIE';RIAStats.DeployJava.browserName2='MSIE';}else if(browser.indexOf('firefox')!=-1){RIAStats.DeployJava.browserName='Netscape Family';RIAStats.DeployJava.browserName2='Firefox';}else if(browser.indexOf('chrome')!=-1){RIAStats.DeployJava.browserName='Netscape Family';RIAStats.DeployJava.browserName2='Chrome';}else if(browser.indexOf('safari')!=-1){RIAStats.DeployJava.browserName='Netscape Family';RIAStats.DeployJava.browserName2='Safari';}else if(browser.indexOf('mozilla')!=-1){RIAStats.DeployJava.browserName='Netscape Family';RIAStats.DeployJava.browserName2='Other';}else if(browser.indexOf('opera')!=-1){RIAStats.DeployJava.browserName='Netscape Family';RIAStats.DeployJava.browserName2='Opera';}else{RIAStats.DeployJava.browserName='?';RIAStats.DeployJava.browserName2='unknown';}
if(RIAStats.DeployJava.debug){alert('Detected browser name:'+RIAStats.DeployJava.browserName+', '+RIAStats.DeployJava.browserName2);}}
return RIAStats.DeployJava.browserName;},testUsingActiveX:function(version){var objectName='JavaWebStart.isInstalled.'+version+'.0';if(!ActiveXObject){if(RIAStats.DeployJava.debug){alert('Browser claims to be IE, but no ActiveXObject object?');}
return false;}
try{return(new ActiveXObject(objectName)!=null);}catch(exception){return false;}},testForMSVM:function(){var clsid='{08B0E5C0-4FCB-11CF-AAA5-00401C608500}';if(typeof oClientCaps!='undefined'){var v=oClientCaps.getComponentVersion(clsid,"ComponentID");if((v=='')||(v=='5,0,5000,0')){return false;}else{return true;}}else{return false;}},testUsingMimeTypes:function(version){if(!navigator.mimeTypes){if(RIAStats.DeployJava.debug){alert('Browser claims to be Netscape family, but no mimeTypes[] array?');}
return false;}
for(var i=0;i<navigator.mimeTypes.length;++i){s=navigator.mimeTypes[i].type;var m=s.match(/^application\/x-java-applet\x3Bversion=(1\.8|1\.7|1\.6|1\.5|1\.4\.2)$/);if(m!=null){if(RIAStats.DeployJava.compareVersions(m[1],version)){return true;}}}
return false;},testUsingPluginsArray:function(version){if((!navigator.plugins)||(!navigator.plugins.length)){return false;}
var platform=navigator.platform.toLowerCase();for(var i=0;i<navigator.plugins.length;++i){s=navigator.plugins[i].description;if(s.search(/^Java Switchable Plug-in (Cocoa)/)!=-1){if(RIAStats.DeployJava.compareVersions("1.5.0",version)){return true;}}else if(s.search(/^Java/)!=-1){if(platform.indexOf('win')!=-1){if(RIAStats.DeployJava.compareVersions("1.5.0",version)||RIAStats.DeployJava.compareVersions("1.6.0",version)){return true;}}}}
if(RIAStats.DeployJava.compareVersions("1.5.0",version)){return true;}
return false;},compareVersions:function(installed,required){var a=installed.split('.');var b=required.split('.');for(var i=0;i<a.length;++i){a[i]=Number(a[i]);}
for(var i=0;i<b.length;++i){b[i]=Number(b[i]);}
if(a.length==2){a[2]=0;}
if(a[0]>b[0])return true;if(a[0]<b[0])return false;if(a[1]>b[1])return true;if(a[1]<b[1])return false;if(a[2]>b[2])return true;if(a[2]<b[2])return false;return true;},writePluginTag:function(){var browser=RIAStats.DeployJava.getBrowser();if(browser=='MSIE'){document.write('<'+'object classid="clsid:CAFEEFAC-DEC7-0000-0000-ABCDEFFEDCBA" '+'id="deployJavaPlugin" width="0" height="0">'+'<'+'/'+'object'+'>');}else if(browser=='Netscape Family'&&RIAStats.DeployJava.allowPlugin()){RIAStats.DeployJava.writeEmbedTag();}},refresh:function(){navigator.plugins.refresh(false);var browser=RIAStats.DeployJava.getBrowser();if(browser=='Netscape Family'&&RIAStats.DeployJava.allowPlugin()){var plugin=document.getElementById('deployJavaPlugin');if(plugin==null){RIAStats.DeployJava.writeEmbedTag();}}},writeEmbedTag:function(){var written=false;if(navigator.mimeTypes!=null){for(var i=0;i<navigator.mimeTypes.length;i++){if(navigator.mimeTypes[i].type==RIAStats.DeployJava.mimeType){if(navigator.mimeTypes[i].enabledPlugin){document.write('<'+'embed id="deployJavaPlugin" type="'+
RIAStats.DeployJava.mimeType+'" hidden="true" />');written=true;}}}
if(!written)for(var i=0;i<navigator.mimeTypes.length;i++){if(navigator.mimeTypes[i].type==RIAStats.DeployJava.oldMimeType){if(navigator.mimeTypes[i].enabledPlugin){document.write('<'+'embed id="deployJavaPlugin" type="'+
RIAStats.DeployJava.oldMimeType+'" hidden="true" />');}}}}},do_initialize:function(){RIAStats.DeployJava.writePluginTag();if(RIAStats.DeployJava.locale==null){var loc=null;if(loc==null)try{loc=navigator.userLanguage;}catch(err){}
if(loc==null)try{loc=navigator.systemLanguage;}catch(err){}
if(loc==null)try{loc=navigator.language;}catch(err){}
if(loc!=null){loc.replace("-","_")
RIAStats.DeployJava.locale=loc;}}}};RIAStats.DeployJava.do_initialize();RIAStats.addEvent(window,'load',RIAStats.init);