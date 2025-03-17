import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TravelRouteService } from './travel-route.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class AppComponent implements OnInit {
  routes: { origin: string; destination: string; price: number }[] = [];
  origin: string = '';      
  destination: string = ''; 
  price: number = 0;        
  searchOrigin: string = ''; 
  searchDestination: string = ''; 
  bestRoute: string = '';  
  message: string = '';    

  editing: boolean = false;

  constructor(private travelRouteService: TravelRouteService) {}

  ngOnInit(): void {
    this.getRoutes();
  }

  getRoutes(): void {
    this.travelRouteService.getRoutes().subscribe({
      next: (data) => this.routes = data,
      error: (err) => this.message = 'Erro ao carregar rotas: ' + err.message
    });
  }

  insertRoute(): void {
    if (!this.origin || !this.destination || this.price <= 0) {
      this.message = "Preencha todos os campos corretamente!";
      return;
    }

    const newRoute = { 
      origin: this.origin.trim().toUpperCase(), 
      destination: this.destination.trim().toUpperCase(), 
      price: this.price 
    };

    this.travelRouteService.insertRoute(newRoute).subscribe({
      next: () => {
        this.message = 'Rota cadastrada com sucesso!';
        this.getRoutes();
        this.clearFields();
      },
      error: (err) => this.message = 'Erro ao cadastrar rota: ' + err.message
    });
  }

  updateRoute(): void {
    if (this.price <= 0) {
      this.message = "O preço deve ser maior que zero!";
      return;
    }

    const updatedRoute = { 
      origin: this.origin, 
      destination: this.destination, 
      price: this.price 
    };

    this.travelRouteService.updateRoute(updatedRoute).subscribe({
      next: () => {
        this.message = 'Rota atualizada com sucesso!';
        this.getRoutes();
        this.clearFields();
      },
      error: (err) => this.message = 'Erro ao atualizar rota: ' + err.message
    });
  }

  editRoute(route: any): void {
    this.origin = route.origin;
    this.destination = route.destination;
    this.price = route.price;
    this.editing = true;
  }

  cancelEdit(): void {
    this.clearFields();
  }

  deleteRoute(origin: string, destination: string): void {
    this.travelRouteService.deleteRoute(origin, destination).subscribe({
      next: () => {
        this.message = 'Rota excluída com sucesso!';
        this.getRoutes();
      },
      error: (err) => this.message = 'Erro ao excluir rota: ' + err.message
    });
  }

  getBestRoute(): void {
    if (!this.searchOrigin || !this.searchDestination) {
      this.message = "Preencha os campos para buscar a melhor rota!";
      return;
    }

    this.travelRouteService.getBestRoute(this.searchOrigin, this.searchDestination).subscribe({
      next: (data) => {
        this.bestRoute = data;
        this.message = '';
      },
      error: (err) => this.message = 'Erro ao buscar melhor rota: ' + err.message
    });
  }

  clearFields(): void {
    this.origin = '';
    this.destination = '';
    this.price = 0;
    this.editing = false;
  }
}
