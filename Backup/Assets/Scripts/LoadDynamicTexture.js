
function Start () {
	var url = "http://a0.twimg.com/profile_images/1096262460/8f5a0400-ba5d-421a-8a54-a073effd9847_bigger.png"; 

	var myImage : WWW = new WWW (url); 
	yield myImage; 
	renderer.material.mainTexture = myImage.texture; 

}