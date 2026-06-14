import time
from locust import HttpUser, task, between
import random
from datetime import datetime, timedelta

class LuxGarageUser(HttpUser):
    """
    Represents a virtual user of the LuxGarage system for load testing.
    This user performs common actions such as browsing vehicles and creating rentals.
    """
    wait_time = between(1, 5)

    @task(10)
    def get_vehicles(self):
        """Simulates browsing the complete list of vehicles."""
        self.client.get("/api/Vehicles")

    @task(5)
    def get_vehicles_by_id(self):
        """Simulates viewing details of a specific, randomly selected vehicle."""
        random_id = random.randint(1, 5)
        self.client.get(f"/api/Vehicles/{random_id}")

    @task(3)
    def get_vehicles_filtered(self):
        """Simulates searching for vehicles with specific brands and sorting criteria."""
        brands = ["BMW", "Audi", "Volkswagen", "Porsche"]
        sorting_fields = ["mileage", "model", "horsepower"]
        random_sorting_field = random.choice(sorting_fields)
        random_brand = random.choice(brands)
        request = {"SearchTerm": random_brand, "SortBy": random_sorting_field}
        self.client.get("/api/Vehicles", params=request)

    def on_start(self):
        """
        Initializes the user session by authenticating with the API.
        This method is called once for each virtual user when they start.
        """
        login_data = {"Email": "admin@luxgarage.com" , "Password": "admin123"}
        response = self.client.post("/api/Auth/login", json=login_data)
        if response.status_code == 200:
            data = response.json()
            access_token = data["token"]
            self.client.headers.update({"Authorization": f"Bearer {access_token}"})

    @task(1)
    def create_rental(self):
        """
        Simulates the process of creating a new vehicle rental.
        Handles both successful bookings (201) and business-level validation errors (400)
        as successful outcomes for load testing purposes.
        """
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
                # Business error (e.g. car already rented) is considered a valid response, not a system failure.
                response.success()
            else:
                response.failure(f"Unexpected status code: {response.status_code}")