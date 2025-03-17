Instruções para executar:

Back: no app.settings alterar a connectionstring de acordo com o BD local para rodar a migrations: "ConnectionStrings": { "DefaultConnection": "Server=LUANMURARI-ACER;Database=TravelRoute;Trusted_Connection=True;TrustServerCertificate=True;" }

Front: no arquivo travel-route.service.ts, trocar a porta caso necessário na linha 9 para a porta da API: - private apiUrl = 'https://localhost:44300/api/TravelRoute';
