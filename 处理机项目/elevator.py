import sys
import time
from functools import partial

from PyQt5.QtCore import QThread, pyqtSignal
from PyQt5.QtWidgets import *
from PyQt5 import QtCore, QtGui, QtWidgets


# 大字体（上下箭头专用）
fontLarge = QtGui.QFont("等线", 48)
fontLarge.setBold(True)

# 较大的字体
fontBig = QtGui.QFont("等线", 14, 75)
fontBig.setBold(True)

# 较小的字体
fontSmall = QtGui.QFont("等线", 11, 50)

# 数字按钮样式
numButtonStyle = "QPushButton{\n" \
                 "background-color: rgb(100, 100, 100);\n" \
                 "color: white;\n" \
                 "border-radius: 10px;\n" \
                 "border: 1px white;\n" \
                 "border-style: outset;\n" \
                 "}\n" \
                 "QPushButton:hover{\n" \
                 "background-color: rgb(77, 199, 176);\n" \
                 "}\n" \
                 "QPushButton:pressed{\n" \
                 "background-color: green;\n" \
                 "color: white;\n" \
                 "}" \
                 "QPushButton:disabled{\n" \
                 "background-color: rgb(200, 200, 200);\n" \
                 "color: white;\n" \
                 "}"

# 数字按钮样式：已按下
numButtonClickedStyle = "QPushButton{\n" \
                        "background-color: rgb(100, 100, 100);\n" \
                        "color: rgb(77, 199, 176);\n" \
                        "border-radius: 10px;\n" \
                        "border: 5px rgb(77, 199, 176);\n" \
                        "border-style: outset;\n" \
                        "}" \
                        "QPushButton:disabled{\n" \
                        "background-color: rgb(200, 200, 200);\n" \
                        "color: white;\n" \
                        "}"

# 报警按钮样式
alertButtonStyle = "QPushButton{\n" \
                   "background-color: red;\n" \
                   "color: white;\n" \
                   "border-radius: 10px;\n" \
                   "border: 1px white;\n" \
                   "border-style: outset;\n" \
                   "}\n" \
                   "QPushButton:hover{\n" \
                   "background-color: yellow;\n" \
                   "}\n" \
                   "QPushButton:pressed{\n" \
                   "background-color: red;\n" \
                   "color: white;\n" \
                   "}"

# 报警按钮样式：已按下
alertButtonClickedStyle = "QPushButton{\n" \
                          "background-color: orange;\n" \
                          "color: white;\n" \
                          "border-radius: 10px;\n" \
                          "border: 1px white;\n" \
                          "border-style: outset;\n" \
                          "}\n" \
                          "QPushButton:hover{\n" \
                          "background-color: yellow;\n" \
                          "}\n" \
                          "QPushButton:pressed{\n" \
                          "background-color: orange;\n" \
                          "color: white;\n" \
                          "}"

# 楼道内按钮样式
corridorButtonStyle = "QPushButton{\n" \
                      "background-color: rgb(100, 100, 100);\n" \
                      "color: white;\n" \
                      "border-radius: 5px;\n" \
                      "border: 1px white;\n" \
                      "border-style: outset;\n" \
                      "}\n" \
                      "QPushButton:hover{\n" \
                      "background-color: rgb(85, 155, 213);\n" \
                      "}\n" \
                      "QPushButton:pressed{\n" \
                      "background-color: blue;\n" \
                      "color: white;\n" \
                      "}"

# 楼道内按钮样式：已按下
corridorButtonClickedStyle = "QPushButton{\n" \
                             "background-color: rgb(100, 100, 100);\n" \
                             "color: rgb(155, 218, 253);\n" \
                             "border-radius: 5px;\n" \
                             "border: 3px rgb(155, 218, 253);\n" \
                             "border-style: outset;\n" \
                             "}"

# LCD显示样式
LCDStyle = "QLCDNumber{\n" \
           "background: black;\n" \
           "color: rgb(77, 199, 176);\n" \
           "}"

# 电梯状态标签样式
stateLabelStyle = "QLabel{\n" \
                  "background-color: black;\n" \
                  "color: rgb(77, 199, 176);\n" \
                  "}"

# 电梯状态标签样式：报警停止
stateAlertLabelStyle = "QLabel{\n" \
                       "background-color: red;\n" \
                       "color: white;\n" \
                       "border-radius: 5px;\n" \
                       "}"

# 电梯状态标签样式：开门
stateOpenLabelStyle = "QLabel{\n" \
                      "background-color: rgb(77, 199, 176);\n" \
                      "color: white;\n" \
                      "}"

# 电梯名称标签样式
markLabelStyle = "QLabel{\n" \
                 "background-color: black;\n" \
                 "color: rgb(77, 199, 176);\n" \
                 "border-radius: 10px;\n" \
                 "}"

# 外部按钮标签样式
corridorLabelStyle = "QLabel{\n" \
                     "background-color: black;\n" \
                     "color: rgb(85, 155, 213);\n" \
                     "border-radius: 10px;\n" \
                     "}"


class MyWindow(QWidget):
    def __init__(self):
        super(MyWindow, self).__init__()
        self.init_interface()

    def init_interface(self):
        # 一、给主窗口命名并设置大小
        self.setObjectName("MainWindow")
        self.resize(1504, 895)

        # 二、设置5部电梯
        for elev in range(5):
            self.gridLayoutWidget = QtWidgets.QWidget(self)  # 新建一个Widget实例，一部电梯一个，存放网格布局
            self.gridLayoutWidget.setGeometry(QtCore.QRect(230 * elev + 30, 140, 201, 631))  # 设置布局位置和尺寸
            self.gridLayoutWidget.setObjectName(f"gridLayoutWidget{elev + 1}")  # 给Widget实例命名

            # 1、设置存放数字键的网格布局
            self.elevator = QtWidgets.QGridLayout(self.gridLayoutWidget)  # 新建一个网格布局，一部电梯一个，存放数字键
            self.elevator.setSizeConstraint(QtWidgets.QLayout.SetNoConstraint)
            self.elevator.setContentsMargins(0, 0, 0, 0)
            self.elevator.setSpacing(2)
            self.elevator.setObjectName(f"Elevator{elev + 1}")  # 给布局命名

            # 2、设置20个数字键
            for btn in range(20):
                self.button = QtWidgets.QPushButton(self.gridLayoutWidget)  # 新建一个按钮
                sizePolicy = QtWidgets.QSizePolicy(QtWidgets.QSizePolicy.Expanding, QtWidgets.QSizePolicy.Expanding)
                self.button.setSizePolicy(sizePolicy)  # 设置尺寸为伸展
                self.button.setFont(fontBig)  # 设置按钮字体
                self.button.setStyleSheet(numButtonStyle)  # 设置按钮样式
                self.button.setObjectName(f"E{elev + 1}F{btn + 1}")  # 给按钮命名
                self.button.setText(f"{btn + 1}")  # 设置按钮文本
                self.button.clicked.connect(partial(set_elev_goal, elev + 1, btn + 1))  # 给按钮挂接SetGoal方法

                if (btn + 1) % 2 == 1:
                    self.LINE = int(-0.5 * (btn + 1) + 11.5)
                    self.ROW = 1
                else:
                    self.LINE = int(-0.5 * (btn + 1) + 12)
                    self.ROW = 2
                self.elevator.addWidget(self.button, self.LINE, self.ROW, 1, 1)  # 向当前布局添加设置好的按钮

            # 3、设置报警键
            self.button = QtWidgets.QPushButton(self.gridLayoutWidget)  # 新建一个按钮
            sizePolicy = QtWidgets.QSizePolicy(QtWidgets.QSizePolicy.Expanding, QtWidgets.QSizePolicy.Expanding)
            self.button.setSizePolicy(sizePolicy)  # 设置尺寸为伸展
            self.button.setFont(fontBig)  # 设置按钮字体
            self.button.setStyleSheet(alertButtonStyle)  # 设置按钮样式
            self.button.setObjectName(f"E{elev + 1}Alert")  # 给按钮命名
            self.button.setText("Alert")  # 设置按钮文本
            self.button.clicked.connect(partial(elev_alert, elev + 1))  # 给按钮挂接ElevAlert方法
            self.elevator.addWidget(self.button, 12, 1, 1, 2)  # 向当前布局添加设置好的按钮

        # 三、设置存放外部按钮的布局
        self.gridLayoutWidget = QtWidgets.QWidget(self)  # 新建一个Widget实例，存放网格布局
        self.gridLayoutWidget.setGeometry(QtCore.QRect(1230, 30, 221, 741))  # 设置布局位置和尺寸
        self.gridLayoutWidget.setObjectName("gridLayoutWidget6")  # 给Widget实例命名
        self.Floor = QtWidgets.QGridLayout(self.gridLayoutWidget)  # 新建一个网格布局，存放外部按钮
        self.Floor.setContentsMargins(0, 0, 0, 0)
        self.Floor.setSpacing(0)
        self.Floor.setObjectName("Floor")  # 给布局命名

        # 四、设置上下各20个外部按钮
        for dir in range(2):
            for lev in range(20):
                self.button = QtWidgets.QPushButton(self)  # 新建一个按钮
                sizePolicy = QtWidgets.QSizePolicy(QtWidgets.QSizePolicy.Expanding, QtWidgets.QSizePolicy.Expanding)
                self.button.setSizePolicy(sizePolicy)  # 设置尺寸为伸展
                self.button.setFont(fontSmall)  # 设置按钮字体
                self.button.setStyleSheet(corridorButtonStyle)  # 设置按钮样式
                self.button.setObjectName(f"{'Up' if dir == 0 else 'Down'}{lev + 1}")  # 给按钮命名
                self.button.setText(f"{lev + 1}{'▲' if dir == 0 else '▼'}")  # 设置按钮文本
                self.button.clicked.connect(partial(
                    set_corridor_goal_up if dir == 0 else set_corridor_goal_down, lev + 1))  # 根据按钮的方向挂接对应方法
                self.Floor.addWidget(self.button, 19 - lev, 2 - dir, 1, 1)  # 向当前布局添加设置好的按钮

        # 五、设置5部电梯的其余配件
        for elev in range(5):
            # 1、设置LCD数码显示
            self.ElevLCD = QtWidgets.QLCDNumber(self)  # 新建一个LCD，一部电梯一个，直接属于主窗口
            self.ElevLCD.setGeometry(QtCore.QRect(230 * elev + 130, 30, 101, 101))  # 设置LCD位置和尺寸
            self.ElevLCD.setStyleSheet(LCDStyle)  # 设置LCD样式
            self.ElevLCD.setDigitCount(2)  # 设置LCD显示的总位数
            self.ElevLCD.setSegmentStyle(QtWidgets.QLCDNumber.Flat)  # 设置数码显示效果为Flat
            self.ElevLCD.setProperty("intValue", 1)  # 设置LCD显示初始值
            self.ElevLCD.setObjectName(f"ElevLCD{elev + 1}")  # 给LCD命名

            # 2、设置电梯状态标签
            self.ElevState = QtWidgets.QLabel(self)  # 新建一个标签，一部电梯一个，直接属于主窗口
            self.ElevState.setGeometry(QtCore.QRect(230 * elev + 30, 30, 101, 101))  # 设置标签位置和尺寸
            self.ElevState.setFont(fontBig)  # 设置标签字体
            self.ElevState.setStyleSheet(stateLabelStyle)  # 设置标签样式
            self.ElevState.setAlignment(QtCore.Qt.AlignCenter)  # 设置标签对齐方式为居中
            self.ElevState.setObjectName(f"ElevState{elev + 1}")  # 给标签命名
            self.ElevState.setText("Stay")  # 设置标签文本

            # 3、设置电梯标签
            self.ElevMark = QtWidgets.QLabel(self)  # 新建一个标签，一部电梯一个，直接属于主窗口
            self.ElevMark.setGeometry(QtCore.QRect(230 * elev + 30, 780, 201, 51))  # 设置标签位置和尺寸
            self.ElevMark.setFont(fontBig)  # 设置标签字体
            self.ElevMark.setStyleSheet(markLabelStyle)  # 设置标签样式
            self.ElevMark.setAlignment(QtCore.Qt.AlignCenter)  # 设置标签对齐方式为居中
            self.ElevMark.setObjectName(f"ElevMark{elev + 1}")  # 给标签命名
            self.ElevMark.setText(f"Elevator{elev + 1}")  # 设置标签文本

        # 六、设置外部按钮标签
        self.CorridorMark = QtWidgets.QLabel(self)  # 新建一个标签，共一个，直接属于主窗口
        self.CorridorMark.setGeometry(QtCore.QRect(1230, 780, 221, 51))  # 设置标签位置和尺寸
        self.CorridorMark.setFont(fontBig)  # 设置标签字体
        self.CorridorMark.setLayoutDirection(QtCore.Qt.LeftToRight)
        self.CorridorMark.setStyleSheet(corridorLabelStyle)  # 设置标签样式
        self.CorridorMark.setAlignment(QtCore.Qt.AlignCenter)  # 设置标签对齐方式为居中
        self.CorridorMark.setObjectName("CorridorMark")  # 给标签命名
        self.CorridorMark.setText("Corridor")  # 设置标签文本

        # 七、收尾工作
        self.setWindowTitle("Elevator-Dispatching Simulator v1.0")  # 设置主窗口标题
        self.show()  # 显示主窗口


class ElevThread(QThread):
    signal = pyqtSignal(int)  # 信号对象实例化，带一个整型参数

    def __init__(self, num):
        super(ElevThread, self).__init__()
        self.elev_num = num  # 接收的参数转为成员：电梯编号
        self.signal.connect(update_state)  # 信号与槽函数update_state连接

    def run(self):
        while 1:  # 线程运行过程处在永真循环中
            if not alert[self.elev_num - 1] and should_open[self.elev_num - 1]:  # 若该电梯没有报警且应当开门
                # 找到电梯所在楼层的楼道按钮、数字键和对应电梯的状态标签
                up_button = mainWindow.findChild(QPushButton, f"Up{floor[self.elev_num - 1]}")
                down_button = mainWindow.findChild(QPushButton, f"Down{floor[self.elev_num - 1]}")
                num_button = mainWindow.findChild(QPushButton, f"E{self.elev_num}F{floor[self.elev_num - 1]}")
                label = mainWindow.findChild(QLabel, f"ElevState{self.elev_num}")

                # 设置开门效果
                up_button.setStyleSheet(corridorButtonStyle)  # 楼道上键恢复状态
                down_button.setStyleSheet(corridorButtonStyle)  # 楼道下键恢复状态
                num_button.setStyleSheet(numButtonStyle)  # 电梯内数字键恢复状态
                label.setFont(fontBig)  # 设置电梯标签字体
                label.setText("Open")  # 设置电梯标签文本
                label.setStyleSheet(stateOpenLabelStyle)  # 设置电梯标签样式

                # 将开门效果延时
                time.sleep(2)

                # 消除开门效果
                label.setStyleSheet(stateLabelStyle)  # 设置电梯标签样式
                label.setText("Stay")  # 设置电梯标签文本
                should_open[self.elev_num - 1] = False  # 将开门标志设为假

            self.signal.emit(self.elev_num)  # 信号发射参数给槽函数update_state，通知其进行常规维护
            time.sleep(1)


def update_state(elev_num):
    if not alert[elev_num - 1]:  # 只有电梯在运行才能改变电梯的状态

        # 一、改变电梯楼层
        if state[elev_num - 1] == 0:
            pass
        elif state[elev_num - 1] == -1:
            floor[elev_num - 1] -= 1
        else:
            floor[elev_num - 1] += 1
        mainWindow.findChild(QLCDNumber, f"ElevLCD{elev_num}").display(floor[elev_num - 1])  # 数码显示更新

        # 二、电梯状态标签更新
        set_elev_label(elev_num)

        # 三、从各目标集合中移除该层
        if (floor[elev_num - 1] in elevator_goal[elev_num - 1]) or (floor[elev_num - 1] in corridor_goal):
            should_open[elev_num - 1] = 1  # 达到目标，设开门标志为真，使线程进行特殊情况的维护
            corridor_goal.discard(floor[elev_num - 1])  # 从楼道里的请求中删除当前楼层

        elevator_goal[elev_num - 1].discard(floor[elev_num - 1])  # 从电梯的目标楼层集合中删除当前楼层

        # 四、改变电梯的状态
        goal_length = len(elevator_goal[elev_num - 1])
        max_goal = -1 if goal_length == 0 else max(elevator_goal[elev_num - 1])
        min_goal = 100 if goal_length == 0 else min(elevator_goal[elev_num - 1])

        # 1、如果当前状态是向下
        if state[elev_num - 1] == -1:
            if goal_length == 0:
                state[elev_num - 1] = 0  # 电梯没有目标可去，状态设为停止
            elif min_goal > floor[elev_num - 1]:
                state[elev_num - 1] = 1  # 电梯最小的目标层数比当前层数还高，状态设为上升

        # 2、如果当前状态是向上
        if state[elev_num - 1] == 1:
            if goal_length == 0:
                state[elev_num - 1] = 0   # 电梯没有目标可去，状态设为停止
            elif max_goal < floor[elev_num - 1]:
                state[elev_num - 1] = -1  # 电梯最大的目标层数比当前层数还低，状态设为下降

        # 3、如果当前状态是静止且有目标层数
        if state[elev_num - 1] == 0 and goal_length != 0:
            if max_goal > floor[elev_num - 1]:
                state[elev_num - 1] = 1   # 电梯最大的目标层数比当前层数高，状态设为上升
            if min_goal < floor[elev_num - 1]:
                state[elev_num - 1] = -1  # 电梯最小的目标层数比当前层数低，状态设为下降


def elev_alert(elev_num):
    # 找到对应电梯的报警键和状态标签
    button = mainWindow.findChild(QPushButton, f"E{elev_num}Alert")
    label = mainWindow.findChild(QLabel, f"ElevState{elev_num}")

    # 未处在报警状态，则设置为报警状态并停止
    if not alert[elev_num - 1]:
        alert[elev_num - 1] = True  # 报警状态标志设为真
        for i in range(20):
            mainWindow.findChild(QPushButton, f"E{elev_num}F{i + 1}").setDisabled(True)  # 将数字键设为Disabled状态
        button.setStyleSheet(alertButtonClickedStyle)  # 设置报警键样式
        label.setFont(fontBig)  # 设置状态标签字体
        label.setStyleSheet(stateAlertLabelStyle)  # 设置状态标签样式
        label.setText("Stall")  # 设置状态标签文本
        print(f"\n{elev_num}号电梯发生异常！\n"
              f"正在检索其目标楼层......{elevator_goal[elev_num - 1]}\n"
              f"正在检索当前楼道目标......{corridor_goal}")

        discard_level = set([])  # 存放要分配给其他电梯的目标楼层
        for goal in elevator_goal[elev_num - 1]:  # 检查当前电梯所有目标楼层，若其来源于楼道内请求，则分配给其他电梯
            if goal in corridor_goal:
                floor_gap = []  # 计算五部电梯当前楼层和目标楼层的差值
                for f in range(5):
                    floor_gap.append(2147483647 if alert[f] else abs(floor[f] - goal))  # 报警状态的电梯离目标楼层无限远

                new_elev = floor_gap.index(min(floor_gap))  # 找出距离目标楼层最近的电梯，将该楼层添加到该电梯的目标中
                discard_level.add(goal)
                elevator_goal[new_elev].add(goal)
                print(f"{elev_num}号电梯本要去{goal}楼的任务因中断而分配给了{new_elev + 1}号电梯")
        print()

        for level in discard_level:  # 将要分配给其他电梯的目标楼层从当前电梯的目标楼层集合中删除
            elevator_goal[elev_num - 1].discard(level)
        if len(elevator_goal[elev_num - 1]) == 0:  # 若删完后当前电梯没有目标楼层了，则运行状态设为静止
            state[elev_num - 1] = 0

    # 处在报警状态，则设置为未报警状态并解除停止
    else:
        alert[elev_num - 1] = False  # 报警状态标志设为假
        for i in range(20):
            mainWindow.findChild(QPushButton, f"E{elev_num}F{i + 1}").setDisabled(False)  # 将数字键设为Enabled状态
        button.setStyleSheet(alertButtonStyle)  # 设置报警键样式
        label.setStyleSheet(stateLabelStyle)  # 设置状态标签样式
        set_elev_label(elev_num)  # 更新状态标签样式
        print(f"{elev_num}号电梯已解除报警状态")


def set_elev_label(elev_num):
    label = mainWindow.findChild(QLabel, f"ElevState{elev_num}")  # 找到当前电梯的状态标签

    if state[elev_num - 1] == 0:
        label.setFont(fontBig)  # 设置状态标签字体
        label.setText(f"{'Stall' if alert[elev_num - 1] else 'Stay'}")  # 设置状态标签文本
    else:
        label.setFont(fontLarge)  # 设置状态标签字体
        label.setText(f"{'↑' if state[elev_num - 1] == 1 else '↓'}")  # 设置状态标签文本


def set_elev_goal(elev_num, goal_floor):
    if floor[elev_num - 1] == goal_floor:  # 若当前电梯已在目标楼层，则不做任何事
        return

    mainWindow.findChild(QPushButton, f"E{elev_num}F{goal_floor}").setStyleSheet(numButtonClickedStyle)  # 设置对应数字键样式
    set_elev_label(elev_num)  # 更新状态标签样式
    elevator_goal[elev_num - 1].add(goal_floor)  # 将目标楼层添加到当前电梯的目标队列中
    print(f"{elev_num}号电梯要去{goal_floor}层")


def set_corridor_goal_up(goal_floor):
    for elev in range(5):  # 检查五部电梯，若有停在当前楼层的，使其中一部开门
        if floor[elev] == goal_floor and state[elev] == 0:
            print(f"电梯{elev + 1}已在{goal_floor}层，无需分配任务")
            should_open[elev] = True
            return

    mainWindow.findChild(QPushButton, f"Up{goal_floor}").setStyleSheet(corridorButtonClickedStyle)  # 设置对应外部按键样式

    floor_gap = []  # 计算五部电梯当前楼层和目标楼层的差值
    for f in range(5):
        floor_gap.append(2147483647 if alert[f] else abs(floor[f] - goal_floor))  # 报警状态的电梯离目标楼层无限远

    elev_num = floor_gap.index(min(floor_gap))  # 找出距离目标楼层最近的电梯，将该楼层添加到该电梯的目标中
    elevator_goal[elev_num].add(goal_floor)
    corridor_goal.add(goal_floor)
    print(f"{elev_num + 1}号电梯被分配到{goal_floor}层的任务")


def set_corridor_goal_down(goal_floor):
    for elev in range(5):  # 检查五部电梯，若有停在当前楼层的，使其中一部开门
        if floor[elev] == goal_floor and state[elev] == 0:
            print(f"电梯{elev + 1}已在{goal_floor}层，无需分配任务")
            should_open[elev] = True
            return

    mainWindow.findChild(QPushButton, f"Down{goal_floor}").setStyleSheet(corridorButtonClickedStyle)  # 设置对应外部按键样式

    floor_gap = []  # 计算五部电梯当前楼层和目标楼层的差值
    for f in range(5):
        floor_gap.append(2147483647 if alert[f] else abs(floor[f] - goal_floor))  # 报警状态的电梯离目标楼层无限远

    elev_num = floor_gap.index(min(floor_gap))  # 找出距离目标楼层最近的电梯，将该楼层添加到该电梯的目标中
    elevator_goal[elev_num].add(goal_floor)
    corridor_goal.add(goal_floor)
    print(f"{elev_num + 1}号电梯被分配到{goal_floor}层的任务")


if __name__ == '__main__':
    app = QApplication(sys.argv)
    mainWindow = MyWindow()

    # 各电梯目标楼层的集合
    elevator_goal = []
    for i in range(5):
        elevator_goal.append(set([]))

    # 各电梯是否开门的标志
    should_open = []
    for i in range(5):
        should_open.append(False)

    # 各电梯的状态
    state = []
    for i in range(5):
        state.append(0)

    # 各电梯是否按下了报警键
    alert = []
    for i in range(5):
        alert.append(False)

    # 各电梯的当前楼层
    floor = []
    for i in range(5):
        floor.append(1)

    # 楼道里的请求集合
    corridor_goal = set([])

    # 五个线程对应五部电梯，每隔一定时间检查每部电梯的状态和elevator_goal数组，并作出相应的行动
    elevators = []
    for i in range(5):
        elevators.append(ElevThread(i + 1))
    for i in range(5):
        elevators[i].start()

    # 进入程序的主循环，并通过exit函数确保主循环安全结束(该释放资源的一定要释放)
    sys.exit(app.exec_())
