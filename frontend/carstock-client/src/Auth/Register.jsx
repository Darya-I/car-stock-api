import { useRef, useState, useEffect } from "react"
import { faCheck, faInfoCircle, faTimes } from "@fortawesome/free-solid-svg-icons"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { Field, Fieldset, DataList, Input, Text, Stack, Card, Heading, Button } from "@chakra-ui/react";
import axios from "axios";


const USER_REGEX = /^[a-zA-Z0-9_-]{3,23}$/;
const PSWRD_REGEX = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$/;
const EMAIL_REGEX = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
const REGISTER_URL = '/api/Account/Register';

const Register = () => {
    const userRef = useRef();
    const errRef = useRef();

    const [email, setEmail] = useState('');
    const [validEmail, setValidEmail] = useState(false);
    const [emailFocus, setEmailFocus] = useState(false);

    const [userName, setUser] = useState('');
    const [validName, setValidName] = useState(false);
    const [userFocus, setUserFocus] = useState(false);

    const [pswrd, setPswrd] = useState('');
    const [validPswrd, setValidPswrd] = useState(false);
    const [pswrdFocus, setPswrdFocus] = useState(false);

    const [matchPswrd, setMatchPswrd] = useState('');
    const [validMatch, setValidMatch] = useState(false);
    const [matchFocus, setMatchFocus] = useState(false);

    const [errMsg, setErrMsg] = useState('');
    const [success, setSuccess] = useState(false);


    useEffect(() => {
        if (userRef.current) {
            userRef.current.focus();
        }
    }, [])

    useEffect(() => {
        const result = EMAIL_REGEX.test(email);
        console.log(result);
        console.log(email);
        setValidEmail(result);
    }, [email])

    useEffect(() => {
        const result = USER_REGEX.test(userName);
        console.log(result);
        console.log(userName);
        setValidName(result);
    }, [userName])

    useEffect(() => {
        const result = PSWRD_REGEX.test(pswrd);
        console.log(result);
        console.log(pswrd);
        setValidPswrd(result);
        const match = pswrd === matchPswrd;
        setValidMatch(match);
    }, [pswrd, matchPswrd])

    useEffect(() => {
        setErrMsg('');
    }, [userName, pswrd, matchPswrd])

    const handleSubmit = async (e) => {
        e.preventDefault;
        // если кнопка разблокирована хаком
        const v1 = USER_REGEX.test(userName);
        const v2 = PSWRD_REGEX.test(pswrd);
        const v3 = EMAIL_REGEX.test(email)
        if (!v1 || !v2 || !v3) {
            setErrMsg("Неверный вход");
            return;
        }
        try {
            const response = await axios.post('https://localhost:7087/api/Account/Register',
                JSON.stringify(
                    {
                        email: email,
                        userName: userName,
                        password: pswrd
                    }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            console.log(response.status);
            console.log(JSON.stringify(response));
            setSuccess(true);
            // очистить поля 
        } catch (err) {
            if (!err?.response) {
                setErrMsg('Нет ответа от сервера')
            }
            else if (err.response?.status === 409) {
                setErrMsg('Пользователь с такой почтой уже существует');
            }
            else {
                setErrMsg('Ошибка регистрации');
            }
            errRef.current.focus();
        }

    }

    return (
        <>
            {success ? (
                <section>
                    <h3 style={{ marginBottom: "10px" }}>Вы успешно зарегистрировались</h3>
                    <p style={{ marginTop: "10px" }}>
                        <a href="#">Войти</a>
                    </p>
                </section>

            ) : (
                <div>
                    <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"}
                        aria-live="assertive">{errMsg}</p>
                    <Text textStyle="2xl" fontWeight="semibold" margin="10" color="blue.700">
                        Регистрация
                    </Text>
                    <Card.Root w="md">
                        <Card.Header>
                            <Card.Description textAlign="center">
                                Заполните форму ниже чтобы зарегистрироваться
                            </Card.Description>
                        </Card.Header>
                        <Card.Body>
                            <Stack gap="4" w="auto" >
                                <Field.Root required invalid={!validName && userName}>
                                    <Field.Label htmlFor="username">
                                        Имя пользователя
                                        {validName && (
                                            <span className="valid">
                                                <FontAwesomeIcon icon={faCheck} />
                                            </span>
                                        )}
                                        {!validName && userName && (
                                            <span className="invalid">
                                                <FontAwesomeIcon icon={faTimes} />
                                            </span>
                                        )}
                                        <Field.RequiredIndicator />
                                    </Field.Label>


                                    <Input
                                        type="text"
                                        id="username"
                                        ref={userRef}
                                        autoComplete="off"
                                        value={userName}
                                        onChange={(e) => setUser(e.target.value)}
                                        required
                                        //aria-invalid={validName ? "false" : "true"}
                                        aria-describedby="uidnote"
                                        onFocus={() => setUserFocus(true)}
                                        onBlur={() => setUserFocus(false)}
                                        placeholder="username"
                                    />

                                    {userFocus && userName && !validName && (
                                        <Field.HelperText id="uidnote">
                                            <FontAwesomeIcon icon={faInfoCircle} />{" "}
                                            Только английские буквы. <br />
                                            Длина от 4 до 24 символов. <br />
                                            Должен начинаться с буквы. <br />
                                            Разрешены буквы, цифры а также "_" и "-".
                                        </Field.HelperText >
                                    )}

                                </Field.Root>
                                <Field.Root required invalid={!validEmail && email}>
                                    <Field.Label htmlFor="email">
                                        Почта
                                        {validEmail && (
                                            <span className="valid">
                                                <FontAwesomeIcon icon={faCheck} />
                                            </span>
                                        )}
                                        {!validEmail && email && (
                                            <span className="invalid">
                                                <FontAwesomeIcon icon={faTimes} />
                                            </span>
                                        )}
                                        <Field.RequiredIndicator />
                                    </Field.Label>

                                    <Input
                                        type="text"
                                        id="email"                                        
                                        autoComplete="off"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                        required
                                        //aria-invalid={validEmail ? "false" : "true"}
                                        aria-describedby="uidnote"
                                        onFocus={() => setEmailFocus(true)}
                                        onBlur={() => setEmailFocus(false)}
                                        placeholder="me@example.com"
                                    />

                                    {emailFocus && email && !validEmail && (
                                        <Field.HelperText id="uidnote">
                                            <FontAwesomeIcon icon={faInfoCircle} />{" "}
                                            Только английские буквы. <br />
                                            Почта должна быть в формате <br />
                                            example@mail.com
                                        </Field.HelperText>
                                    )}

                                </Field.Root>

                                <Field.Root required invalid={!validPswrd && pswrd}>
                                    <Field.Label htmlFor="password">
                                        Пароль
                                        {validPswrd && (
                                            <span className="valid">
                                                <FontAwesomeIcon icon={faCheck} />
                                            </span>
                                        )}
                                        {!validPswrd && pswrd && (
                                            <span className="invalid">
                                                <FontAwesomeIcon icon={faTimes} />
                                            </span>
                                        )}
                                        <Field.RequiredIndicator />
                                    </Field.Label>


                                    <Input
                                        type="password"
                                        id="password"
                                        onChange={(e) => setPswrd(e.target.value)}
                                        required
                                        aria-describedby="pswrdnote"
                                        onFocus={() => setPswrdFocus(true)}
                                        onBlur={() => setPswrdFocus(false)}
                                    />

                                    {pswrdFocus && !validPswrd && (
                                        <Field.HelperText id="pswrdnote">
                                            <FontAwesomeIcon icon={faInfoCircle} />{" "}
                                            Только английские буквы. <br />
                                            Длина от 8 до 16 символов. <br />
                                            Должен включать буквы верхнего и нижнего регистра,<br />
                                            цифры и специальные символы
                                        </Field.HelperText >
                                    )}

                                </Field.Root>
                                <Field.Root required invalid={!validMatch && matchPswrd}>
                                    <Field.Label htmlFor="pswrd-confirm">
                                        Подтвердить пароль
                                        {validMatch && matchPswrd && (
                                            <span className="valid">
                                                <FontAwesomeIcon icon={faCheck} />
                                            </span>
                                        )}
                                        {!validMatch && matchPswrd && (
                                            <span className="invalid">
                                                <FontAwesomeIcon icon={faTimes} />
                                            </span>
                                        )}
                                        <Field.RequiredIndicator />
                                    </Field.Label>


                                    <Input
                                        type="password"
                                        id="pswrd-confirm"
                                        onChange={(e) => setMatchPswrd(e.target.value)}
                                        required
                                        aria-describedby="pswrdconfirmnote"
                                        onFocus={() => setMatchFocus(true)}
                                        onBlur={() => setMatchFocus(false)}
                                    />

                                    {matchFocus && !validMatch && (
                                        <Field.HelperText id="pswrdconfirmnote">
                                            <FontAwesomeIcon icon={faInfoCircle} />{" "}
                                            Пароли должны совпадать
                                        </Field.HelperText >
                                    )}

                                </Field.Root>
                            </Stack>
                        </Card.Body>
                        <Card.Footer justifyContent="space-between" flexDirection="column" alignItems="baseline">
                            <Stack direction="row" spasing={4} w="full" justifyContent="space-between">
                                <Button variant="outline">Cancel</Button>
                                <Button disabled={!validName || !validEmail || !validPswrd || !validMatch ? true : false}
                                    variant="ghost" color="blue.700"
                                    onClick={handleSubmit}>
                                    Зарегистрироваться
                                </Button>
                            </Stack>
                            <Text marginTop="2" fontSize="sm" color="gray.500">Уже есть аккаунт?</Text>
                            <Text marginTop="-1" as="a" href="/login" color="blue.500" fontSize="sm">
                                Войти
                            </Text>
                        </Card.Footer>
                    </Card.Root>
                </div>
            )
            }
        </>
    )
}

export default Register;