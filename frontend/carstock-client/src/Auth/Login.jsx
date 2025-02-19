import { Button, Card, Field, Input, Stack, Text } from "@chakra-ui/react";
import axios from "axios";
import { useEffect, useRef, useState } from "react";
import  useAuth  from '../hooks/useAuth'
import { useLocation, useNavigate } from "react-router-dom";

const Login = () => {
    const { setAuth } = useAuth();
    
    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";

    const userRef = useRef();
    const errRef = useRef();

    const [email, setEmail] = useState('');
    const [pswrd, setPswrd] = useState('');
    const [errMsg, setErrMsg] = useState('');

    useEffect(() => {
        if (userRef.current) {
            userRef.current.focus();
        }
    }, [])


    useEffect(() => {
        setErrMsg('');
    }, [email, pswrd])

    const handleSubmit = async (e) => {
        e.preventDefault;
        try {
            const response = await axios.post('https://localhost:7087/api/Auth/Login',
                JSON.stringify(
                    {
                        email: email,
                        password: pswrd
                    }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            const accessToken = response?.data;
            setAuth({ email, pswrd, accessToken});
            setEmail('');
            setPswrd('');
            navigate(from, {replace: true});
        } catch (err) {
            if (!err?.response) {
                setErrMsg("Нет ответа от сервера")
            }
            else if (err.response?.status === 401) {
                setErrMsg("Пользователь не найден")
            }
            else {
                setErrMsg("Ошибка входа");
            }
            errRef.current.focus();
        }
    }


    return (
        <>       
                <div>
                    {/* set err ref here */}
                    <p className={errMsg ? "errmsg" : "offscreen"}
                        aria-live="assertive">{errMsg}</p>
                    <Text textStyle="2xl" fontWeight="semibold" margin="10" color="blue.700">
                        Вход
                    </Text>
                    <Card.Root w="md">
                        <Card.Header>
                            <Card.Description textAlign="center">
                                Заполните форму ниже чтобы войти
                            </Card.Description>
                        </Card.Header>
                        <Card.Body>
                            <Stack gap="4" w="auto">
                                <Field.Root>
                                    <Field.Label htmlFor="email">
                                        Почта
                                    </Field.Label>
                                    <Input
                                        type="text"
                                        id="email"
                                        ref={userRef}
                                        // autoComplete="off"
                                        onChange={(e) => setEmail(e.target.value)}
                                        value={email}
                                        required>
                                    </Input>
                                </Field.Root>
                                <Field.Root>
                                    <Field.Label htmlFor="password">
                                        Пароль
                                    </Field.Label>
                                    <Input
                                        type="password"
                                        id="password"
                                        onChange={(e) => setPswrd(e.target.value)}
                                        value={pswrd}
                                        required>
                                    </Input>
                                </Field.Root>
                            </Stack>
                        </Card.Body>
                        <Card.Footer justifyContent="space-between" flexDirection="column" alignItems="baseline">
                            <Stack direction="row" spacing={4} w="full" justifyContent="space-between">
                                <Button variant="outline">Отмена</Button>
                                <Button
                                    disabled={!email || !pswrd}
                                    variant="ghost" color="blue.700"
                                    onClick={handleSubmit}>
                                    Войти
                                </Button>
                            </Stack>
                            <Text marginTop="2" fontSize="sm" color="gray.500">Нет аккаунта?</Text>
                            <Text marginTop="-1" as="a" href="/register" color="blue.500" fontSize="sm">
                                Зарегистрироваться
                            </Text>
                        </Card.Footer>
                    </Card.Root>
                </div>
        </>
    )
}

export default Login;