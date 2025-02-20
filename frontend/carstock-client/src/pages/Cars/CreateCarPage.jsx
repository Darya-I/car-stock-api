import React, { useState, useEffect } from "react";
import axios from "../../axios/axios"
import { Button, Fieldset, Input, Stack, Text, ButtonGroup } from "@chakra-ui/react"
import { Field } from "../../components/ui/field";
import { Checkbox } from "../../components/ui/checkbox"
import { NativeSelectField, NativeSelectRoot } from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";


const CreateCarPage = () => {
  const [brands, setBrands] = useState([]);
  const [selectedBrand, setSelectedBrand] = useState("");
  const [models, setModels] = useState([]);
  const [selectedModel, setSelectedModel] = useState("");
  const [colors, setColors] = useState([]);
  const [selectedColor, setSelectedColor] = useState("");
  const [amount, setAmount] = useState(0);
  const [isAvailable, setAvailable] = useState(false);


  const navigate = useNavigate();


  useEffect(() => {
    const fetchBrand = async () => {
      try {
        const respone = await axios.get("/api/Brand/GetBrands");
        setBrands(respone.data);

      } catch (error) {
        console.error("Ошибка загрузки марок:", error);
      }
    };
    fetchBrand();
  }, []);

  useEffect(() => {
    const fetchModel = async () => {
      if (!selectedBrand) {
        setModels([]);
        return;
      }
      try {
        const response = await axios.get(`/api/CarModel/GetModelByBrand/${selectedBrand}`);
        setModels(response.data);
      } catch (error) {
        console.error("Ошибка загрузки моделей:", error);
      }
    };
    fetchModel();
  }, [selectedBrand])

  useEffect(() => {
    const fetchColors = async () => {
      if (!selectedModel) {
        setColors([]);
        return;
      }
      try {
        const response = await axios.get("/api/Color/GetColors");
        setColors(response.data);
      } catch (error) {
        console.error("Ошибка загрузки цветов:", error);
      }
    };
    fetchColors();
  }, [selectedModel]);

  const handleSubmit = async () => {
    const carData = {
      brandId: selectedBrand,
      carModelId: selectedModel,
      colorId: selectedColor,
      amount: Number(amount),
      isAvailable: isAvailable,
    };

    try {
      await axios.post("/api/Car/CreateCar", carData);
      alert("Автомобиль добавлен")
    } catch (error) {
      console.error("Ошибка при добавлении автомобиля:", error);
      alert("Не удалось добавить автомобиль. Проверьте введенные данные.");
    }
  };

  return (
    <>
      <Fieldset.Root size="lg" maxW="md">
        <Stack>
          <Fieldset.Legend className="headText">Добавление автомобиля</Fieldset.Legend>
          <Fieldset.HelperText>Выберите характеристики автомобиля и заполните поля</Fieldset.HelperText>
        </Stack>

        <Fieldset.Content>
          <Field label="Марка">
            <NativeSelectRoot>
              <NativeSelectField
                value={selectedBrand}
                onChange={(e) => setSelectedBrand(Number(e.target.value))}
                defaultValue=""
              >
                <option value="" disabled>Выбрать марку</option>

                {brands.map((brand) => (
                  <option key={brand.id} value={brand.id}>
                    {brand.name}
                  </option>
                ))}

              </NativeSelectField>
            </NativeSelectRoot>
          </Field>
          <Field label="Модель">
            <NativeSelectRoot>
              <NativeSelectField
                value={selectedModel}
                onChange={(e) => setSelectedModel(Number(e.target.value))}
                defaultValue=""
              >
                <option value="" disabled>Выбрать модель</option>

                {models.map((model) => (
                  <option key={model.id} value={model.id}>
                    {model.name}
                  </option>
                ))}

              </NativeSelectField>
            </NativeSelectRoot>
          </Field>
          <Field label="Цвет">
            <NativeSelectRoot>
              <NativeSelectField
                value={selectedColor}
                onChange={(e) => setSelectedColor(Number(e.target.value))}
                defaultValue=""
              >
                <option value="" disabled>Выбрать цвет</option>

                {colors.map((color) => (
                  <option key={color.id} value={color.id}>
                    {color.name}
                  </option>
                ))}

              </NativeSelectField>
            </NativeSelectRoot>
          </Field>
          <Field label="Количество (макс. 100)">
            <Input
              type="number"
              placeholder="Введите количество автомобилей"
              onChange={(e) => {
                const value = Math.min(Number(e.target.value), 100);
                setAmount(value);
              }}
              max={100}
            />
          </Field>

          <Field label="Доступность">
            <Stack direction="row" gap="4">
              <Checkbox
                isChecked={isAvailable}
                onChange={() => setAvailable(!isAvailable)}
              />
              <Text>{isAvailable ? "Доступен" : "Недоступен"}</Text>
            </Stack>
          </Field>

        </Fieldset.Content>
      </Fieldset.Root>

      <div style={{ display: "flex", justifyContent: "space-between", margin: "30px" }}>
        <Button variant="ghost" color="blue" onClick={() => navigate(-1)}>Назад</Button>
        <Button variant="solid" color="green" onClick={handleSubmit}>Добавить</Button>
      </div>
    </>
  );
};

export default CreateCarPage;
