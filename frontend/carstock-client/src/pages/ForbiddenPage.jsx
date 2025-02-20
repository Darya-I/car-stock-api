import { Text } from "@chakra-ui/react"

const ForbiddenPage = () => {
    return (
        <>
            <div>
                <Text>Сейчас у вас недостаточно прав для просмотра этой страницы</Text>
                <Text marginTop="-1" as="a" href="/" color="blue.500" fontSize="sm">
                    Вернуться на главную
                </Text>
            </div>
        </>
    )
}

export default ForbiddenPage;