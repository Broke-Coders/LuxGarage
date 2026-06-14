import time
from locust import HttpUser, task, between
import random
from datetime import datetime, timedelta

class LuxGarageUser(HttpUser):
    wait_time = between(1, 5)

    @task
    def get_vehicles(self):
        self.client.get("/api/Vehicles")

    @task
    def get_vehicles_by_id(self):
        random_id = random.randint(1, 5)
        self.client.get(f"/api/Vehicles/{random_id}")

    @task
    def get_vehicles_filtered(self):
        brands = ["BMW", "Audi", "Volkswagen", "Porsche"]
        sorting_fields = ["mileage", "model", "horsepower"]
        random_sorting_field = random.choice(sorting_fields)
        random_brand = random.choice(brands)
        request = {"SearchTerm": random_brand, "SortBy": random_sorting_field}
        self.client.get("/api/Vehicles", params=request)
    

    def on_start(self):
        login_data = {"Email": "admin@luxgarage.com" , "Password": "admin123"}
        response = self.client.post("/api/Auth/login", json=login_data)
        if response.status_code == 200:
            data = response.json()
            access_token = data["token"]
            self.client.headers.update({"Authorization": f"Bearer {access_token}"})

    @task(1)
    def create_rental(self):
        payload = {
                   "VehicleId": random.randint(1, 3), 
                   "CustomerEmail": "test@luxgarage.com", 
                   "StartDate": datetime.now().isoformat(), 
                   "EndDate": (datetime.now() + timedelta(days=3)).isoformat() 
                   }
        with self.client.post("/api/Rental", json=payload, catch_response=True) as response:
            if response.status_code == 201:
                response.success()
            elif response.status_code == 400:
                response.success()
            else:
                response.failure(f"Unexpected status code: {response.status_code}")