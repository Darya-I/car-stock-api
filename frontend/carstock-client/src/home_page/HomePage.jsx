import { Button, Stack } from "@chakra-ui/react";
import { Link } from "react-router-dom";
import './HomePage.css';

function HomePage() { 
    return (
        <>
            <div className="headText">
                <h2>Справочник автомобилей</h2>
            </div>
            <Stack direction="row" gap="4" align="flex-start">
                <Button colorScheme="blue" variant="ghost" asChild>
                    <a href="/cars">Автомобили</a>
                </Button>
                <Button colorScheme="blue" variant="ghost" asChild>
                    <a href="/users">Пользователи</a>
                </Button>
                <Button colorScheme="blue" variant="ghost" asChild>
                    <a href="/account"> Аккаунт</a>
                </Button>
            </Stack>
        </>
    );
}

export default HomePage;
