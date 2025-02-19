import React, { useState, useEffect } from "react";
import axios from "axios";
import { DataList, Button, Stack, Toast, createToaster } from "@chakra-ui/react";
import '../styles/CarsPage.css';
import { useNavigate } from "react-router-dom";
//import { toaster } from "@/components/ui/toaster"

const GetCarsPage = () => {
  const showCarsApi = "https://localhost:7087/api/Car/GetCars";
  const deleteCarApi = "https://localhost:7087/api/Car/DeleteCar";

  const navigate = useNavigate();

  const [cars, setCars] = useState([]);

  const toaster = createToaster({
    duration: 5000,
    max: 3,
  });

  useEffect(() => {
    fetchCars();
  }, []);

  const fetchCars = async () => {
    try {
      const response = await axios.get(showCarsApi);
      setCars(response.data);
    } catch (error) {
      console.error("Ошибка загрузки автомобилей:", error);
    }
  }

  const handleDelete = async (id) => {
    try {
      let isConfirm = confirm("Подтвердить удаление автомобиля");
      if (!isConfirm) return;

      await axios.delete(`${deleteCarApi}/${id}`);
      setCars(cars.filter((car) => car.id !== id));

      toaster.show({
        title: "Автомобиль удален",
        type: "success"
      })


    } catch (error) {
      if(error.response?.status === 401) {
        toaster.create({
          title: "У вас нет прав на удаление",
          type: "error"
        })
      }
      console.error("Ошибка удаления автомобиля:", error);
    }
  };

  return (
    <>
      <div className="headText">
        <h2>Страница автомобилей</h2>
      </div>
      <DataList.Root orientation="horizontal">
        <DataList.Item className="zagolovki">
          <DataList.ItemLabel>Марка</DataList.ItemLabel>
          <DataList.ItemLabel>Модель</DataList.ItemLabel>
          <DataList.ItemLabel>Цвет</DataList.ItemLabel>
          <DataList.ItemLabel>Количество</DataList.ItemLabel>
          <DataList.ItemLabel>Доступность</DataList.ItemLabel>
          <DataList.ItemLabel>Действия</DataList.ItemLabel>
        </DataList.Item>

        {cars.map((car) => (
          <DataList.Item key={car.id}>
            <DataList.ItemValue>{car.brand}</DataList.ItemValue>
            <DataList.ItemValue>{car.carModel}</DataList.ItemValue>
            <DataList.ItemValue>{car.color}</DataList.ItemValue>
            <DataList.ItemValue>{car.amount}</DataList.ItemValue>
            <DataList.ItemValue>{car.isAvailable ? "Доступен" : "Недоступен"}</DataList.ItemValue>
            <DataList.ItemValue>
              <Stack direction="row" spacing={2}>
                <Button
                  variant="outline"
                  color="red.600"
                  onClick={() => handleDelete(car.id)}
                >
                  Удалить
                </Button>
                <Button
                  variant="outline"
                  color="blue.600"
                  onClick={() => navigate(`/edit-car/${car.id}`)}
                >
                  Изменить
                </Button>
              </Stack>
            </DataList.ItemValue>
          </DataList.Item>
        ))}
      </DataList.Root>
      <div style={{ display: "flex", justifyContent: "left", margin: "20px" }}>
        <Button variant="outline" color="blue.600" onClick={() => navigate(-1)}>
          Назад
        </Button>
      </div>
    </>
  );
};

export default GetCarsPage;
