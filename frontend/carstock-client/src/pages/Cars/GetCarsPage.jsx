import React, { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { DataList, Button, Stack } from "@chakra-ui/react";
import "../styles/CarsPage.css";
import { useNavigate } from "react-router-dom";
import { toast, ToastContainer } from "react-toastify";

const GetCarsPage = () => {
  const showCarsApi = "/api/Car/GetCars";
  const deleteCarApi = "/api/Car/DeleteCar";
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();

  const [cars, setCars] = useState([]);

  useEffect(() => {
    fetchCars();
  }, []);

  const fetchCars = async () => {
    try {
      const response = await axiosPrivate.get(showCarsApi);
      setCars(response.data);
    } catch (error) {
      console.error("Ошибка загрузки автомобилей:", error);
    }
  };

  const handleDelete = async (id) => {
    try {
      const isConfirm = confirm("Подтвердить удаление автомобиля");
      if (!isConfirm) return;

      await axiosPrivate.delete(`${deleteCarApi}/${id}`);
      toast.success("Успешно удален", {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "light",
      });
      setCars((prevCars) => prevCars.filter((car) => car.id !== id));
      
    } catch (error) {
      if (error.response?.status === 401) {
        toast.warn("У вас нет прав на это действие", {
          position: "bottom-right",
          autoClose: 5000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "light",
        });
        return;
      }
      toast.error("Ошибка удаления автомобиля", {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "light",
      });
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
                <Button variant="outline" color="red.600" onClick={() => handleDelete(car.id)}>
                  Удалить
                </Button>
                <Button variant="outline" color="blue.600" onClick={() => navigate(`/edit-car/${car.id}`)}>
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
        <ToastContainer />
    </>
  );
};

export default GetCarsPage;
