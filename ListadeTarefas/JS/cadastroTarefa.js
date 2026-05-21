const myForm1 = document.getElementById('novatarefa');
if (myForm1 != null) {
myForm1.addEventListener('submit', function (event) {
    // 1. Prevenir o recarregamento da página ao submeter form
    event.preventDefault();

    fetch('https://localhost:7112/tarefa/cadastrar', {
        method: 'POST', //Para outros métodos, basta alterar aqui. Obs: Delete remove a parte do body e headers, e no get é conforme todos os exemploes feitos na Unidade interação com API 
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            descricao: document.getElementById("descricao").value,
            status: document.getElementById("stts").value
        }),
    }).then(response => {
        console.log(response);
        if (response.status ==401){
            alert ("Faça login antes de cadastrar!");
            window.location.href="tarefas.html";
        }
        response.json();})
        .then(data => {
            console.log(data);
            document.getElementById("respostaTarefa").innerHTML ="<h4>Tarefa cadastrada com sucesso!</h4>";        
        })
});
}

fetch('https://localhost:7112/tarefa',
    { 
        credentials: 'include'   
    }).then(response => {
        console.log(response);
       // if (response.status ==401){
       //     alert ("Faça login antes de cadastrar!");
       //     window.location.href="tarefas.html";
       // }
        return response.json();})
    .then(data => {
        //if(data.length >0){
        console.log(data);
        var resposta = document.getElementById("respostaConsulta");
        resposta.innerHTML = "<h4>Segue a Lista das suas Tarefas</h4> ";
        for (i = 0; i < data.length; i++) {
            resposta.innerHTML += "<li> Usuario: " + data[i].usuario + "</li>";
            resposta.innerHTML += "Descricao: <input type='text' id='descricao"+data[i].id+"' value='" + data[i].tarefa + "'>";
            resposta.innerHTML += "Status: <input type='text' id='stts"+data[i].id+"' value='" + data[i].status + "'>";
            resposta.innerHTML += "<button onclick='editaTarefa("+data[i].id+")'>Editar Tarefa </button>";
            resposta.innerHTML += "<button onclick='deletaTarefa("+data[i].id+")'>Deletar Tarefa </button> <hr>";

       // }
    }
    });

    function deletaTarefa(idTarefa){
        fetch('https://localhost:7112/tarefa/'+idTarefa, {
            method: 'DELETE', 
            credentials: 'include'
  
        }).then(response => {
            alert("Tarefa excluída");
            window.location.href="tarefas.html";
        })
    }

    function editaTarefa (idTarefa){
        fetch('https://localhost:7112/tarefa/'+idTarefa, {
            method: 'PUT',   
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                descricao: document.getElementById("descricao"+idTarefa).value,
                status: document.getElementById("stts"+idTarefa).value
            }),
        }).then(response => {
            if (response.status ==401){
                alert ("Faça login antes de editar!");
                window.location.href="tarefas.html";
            }else{
                alert ("Tarefa editada!");
            }})
           
    }
    function logout() {
    fetch('https://localhost:7112/usuario/logout', { credentials: 'include' })
        .then(response => {
            console.log(response);
            window.location.href = "index.html"
        })
}0