import React, { useState, useEffect } from "react";
import axios from "axios";
import { Button, Stack } from "@chakra-ui/react";
import '../styles/CarsPage.css';
import { useNavigate } from "react-router-dom";


function CarsPage() {
    const navigate = useNavigate();
  return (
    <>
    <div className="headText">
      <h2>Действия с автомобилями </h2>
    </div>
    <Stack direction="row" gap="4" align="flex-start">
      <Button colorScheme="blue" variant="ghost" asChild>
        <a href="/getcars">Посмотреть все автомобили</a>
      </Button>
      <Button colorScheme="blue" variant="ghost" asChild>
        <a href="/createcar">Добавить автомобиль</a>
      </Button>
      <Button color="blue.500" variant="ghost">
        Найти автомобиль по идентификатору
      </Button>
    </Stack>
    <div style={{ display: "flex", justifyContent: "left", margin: "20px" }}>
      <Button size="xs" variant="outline" color="blue.600" onClick={() => navigate(-1)}>
        Назад
      </Button>
    </div>
    </>
  )
}

export default CarsPage;
