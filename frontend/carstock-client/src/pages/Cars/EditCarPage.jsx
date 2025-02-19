import React, { useState, useEffect } from "react";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom";
import { Fieldset, DataList, Input, Text, Stack, Card, Heading, Button } from "@chakra-ui/react";
import { NativeSelectField, NativeSelectRoot } from "@chakra-ui/react";
import { Field } from "../../components/ui/field";
import { Checkbox } from "../../components/ui/checkbox"


const EditCarPage = () => {
    const { id: carId } = useParams();
    const navigate = useNavigate();
    const [brands, setBrands] = useState([]);
    const [selectedBrand, setSelectedBrand] = useState("");
    const [models, setModels] = useState([]);
    const [selectedModel, setSelectedModel] = useState("");
    const [colors, setColors] = useState([]);
    const [selectedColor, setSelectedColor] = useState("");
    const [amount, setAmount] = useState(0);
    const [isAvailable, setAvailable] = useState(false);


    const updateCarApi = "https://localhost:7087/api/Car/UpdateCar";
    const getCarApi = `https://localhost:7087/api/Car/GetCar/${carId}`;

    const [car, setCar] = useState([]);

    useEffect(() => {
        fetchCar();
    }, [carId]);

    const fetchCar = async () => {
        try {
            const response = await axios.get(getCarApi);
            setCar(response.data);
            console.log(car);
        } catch (error) {
            console.error("Ошибка загрузки автомобилей:", error);
        }
    };

    useEffect(() => {
        const fetchBrand = async () => {
            try {
                const respone = await axios.get("https://localhost:7087/api/Brand/GetBrands");
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
                const response = await axios.get(`https://localhost:7087/api/CarModel/GetModelByBrand/${selectedBrand}`);
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
                const response = await axios.get("https://localhost:7087/api/Color/GetColors");
                setColors(response.data);
            } catch (error) {
                console.error("Ошибка загрузки цветов:", error);
            }
        };
        fetchColors();
    }, [selectedModel]);

    const handleSubmit = async () => {
        const carData = {
            id: carId,
            brandId: selectedBrand,
            carModelId: selectedModel,
            colorId: selectedColor,
            amount: Number(amount),
            isAvailable: isAvailable,
        }

        try {
            await axios.put(`${updateCarApi}/${carId}`, carData);
            alert("Автомобиль изменен");
            fetchCar();
        } catch (error) {
            console.error("Ошибка при изменении автомобиля:", error);
            alert("Не удалось изменить автомобиль. Проверьте введенные данные.");
        }
    };

    return (
        <>
            <div style={{ display: "flex", justifyContent: "space-between", gap: "20px" }}>
                {/* Карточка товара */}
                <Card.Root flex="1" display={"flex"}>
                    <Card.Header>
                        <Heading size="md">
                            Идентификатор автомобиля: {car.id}
                        </Heading>
                    </Card.Header>
                    <Card.Body alignContent={"center"}>
                        <DataList.Root orientation="horizontal" divideY="1px" maxW="md">
                            <DataList.Item>
                                <DataList.ItemLabel className="zagolovki">Марка</DataList.ItemLabel>
                                <DataList.ItemValue>{car.brand}</DataList.ItemValue>
                            </DataList.Item>
                            <DataList.Item>
                                <DataList.ItemLabel className="zagolovki">Модель</DataList.ItemLabel>
                                <DataList.ItemValue>{car.carModel}</DataList.ItemValue>
                            </DataList.Item>
                            <DataList.Item>
                                <DataList.ItemLabel className="zagolovki">Цвет</DataList.ItemLabel>
                                <DataList.ItemValue>{car.color}</DataList.ItemValue>
                            </DataList.Item>
                            <DataList.Item>
                                <DataList.ItemLabel className="zagolovki">Количество</DataList.ItemLabel>
                                <DataList.ItemValue>{car.amount}</DataList.ItemValue>
                            </DataList.Item>
                            <DataList.Item>
                                <DataList.ItemLabel className="zagolovki">Доступность</DataList.ItemLabel>
                                <DataList.ItemValue>{car.isAvailable ? "Доступен" : "Недоступен"}</DataList.ItemValue>
                            </DataList.Item>
                        </DataList.Root>
                    </Card.Body>
                </Card.Root>

                {/* Форма добавления автомобиля */}
                <Fieldset.Root size="lg" maxW="md" flex="1">
                    <Stack>
                        <Fieldset.Legend className="headText">Изменение автомобиля</Fieldset.Legend>
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
                                    <option value="" disabled>Выбрать новую марку</option>
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
            </div>
            <Stack direction="row" justifyContent="space-between" width="100%" mt="6">
                <Button variant="outline" colorPalette="blue" onClick={() => navigate(-1)}>
                    Назад
                </Button>
                <Button variant="outline" colorPalette="green" onClick={handleSubmit}>
                    Изменить
                </Button>
            </Stack>
        </>
    );
};

export default EditCarPage;