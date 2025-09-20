import curses
import random
import time


def main(stdscr):
    # Настройки
    curses.curs_set(0)  # Скрываем курсор
    stdscr.nodelay(1)  # Неблокирующий ввод
    stdscr.timeout(100)  # Таймаут обновления экрана

    # Размеры экрана
    sh, sw = stdscr.getmaxyx()
    w = curses.newwin(sh, sw, 0, 0)
    w.keypad(1)

    # Начальная позиция змеи
    snake = [[sh // 2, sw // 4]]
    food = [sh // 2, sw // 2]

    # Направление движения
    key = curses.KEY_RIGHT
    direction = key

    # Счет
    score = 0

    # Отображаем первую еду
    w.addch(food[0], food[1], curses.ACS_PI)

    while True:
        # Получаем следующий ключ
        next_key = w.getch()
        if next_key != -1:
            if next_key in [
                curses.KEY_UP,
                curses.KEY_DOWN,
                curses.KEY_LEFT,
                curses.KEY_RIGHT,
            ]:
                direction = next_key

        # Определяем новую голову
        head = snake[0].copy()

        if direction == curses.KEY_UP:
            head[0] -= 1
        elif direction == curses.KEY_DOWN:
            head[0] += 1
        elif direction == curses.KEY_LEFT:
            head[1] -= 1
        elif direction == curses.KEY_RIGHT:
            head[1] += 1

        # Проверяем столкновение с границами
        if head[0] in [0, sh - 1] or head[1] in [0, sw - 1] or head in snake[1:]:
            break

        # Добавляем новую голову
        snake.insert(0, head)

        # Проверяем, съела ли змея еду
        if head == food:
            score += 1
            # Генерируем новую еду
            food = None
            while food is None:
                new_food = [random.randint(1, sh - 2), random.randint(1, sw - 2)]
                food = new_food if new_food not in snake else None
            w.addch(food[0], food[1], curses.ACS_PI)
        else:
            # Удаляем хвост
            tail = snake.pop()
            w.addch(tail[0], tail[1], " ")

        # Рисуем змею
        w.addch(snake[0][0], snake[0][1], curses.ACS_CKBOARD)

        # Обновляем счет
        w.addstr(0, 2, f"Score: {score}")
        w.refresh()

    # Конец игры
    w.addstr(sh // 2, sw // 2 - 5, f"GAME OVER! Score: {score}")
    w.refresh()
    time.sleep(2)


if __name__ == "__main__":
    curses.wrapper(main)
