var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var players={};

app.get('/', function(req, res){
  res.sendFile(__dirname + '/index.html');
});

http.listen(3000, function(){
  console.log('listening on *:3000');
});

io.on('connection', function(socket){
	console.log('a user connected');

	socket.on('disconnect', function(){
		//players.splice(socket.id,1);
	
	console.log('user disconnected '+socket.id);
	});

socket.on('newPlayer', function() {

	var player={
	      	x: 0,
	      	y: 0	
      };
    players[socket.id]=player;
 
  // console.log("newPlayer:"+JSON.stringify(players[socket.id]));
  });

  socket.on('move', function(data) {
  	var player =players[socket.id] ||{};
   
   // console.log("Data:"+JSON.stringify(data));
      player.x =player.x+(data.x*0.1);
      player.y =player.y+(data.y*0.1);
    console.log("Player:"+JSON.stringify(player));
  });
});


setInterval(function() {
  console.log("PLAYERS"+JSON.stringify(players));
  io.emit('state', players);
  
}, 1000 / 60);