import sys
import time
import random
from functools import partial
from queue import Queue

from PyQt5.QtCore import QThread, pyqtSignal
from PyQt5.QtWidgets import *
from PyQt5 import QtCore, QtGui, QtWidgets


# 小号字体（表格专用）
fontSmall = QtGui.QFont("黑体", 9)

# 大号字体
fontBig = QtGui.QFont("黑体", 12)

# 记录表格样式
recordTableStyle = "QTableWidget{\n" \
                   "background-color: rgb(35, 38, 41);\n" \
                   "color:rgb(29, 233, 182);\n" \
                   "gridline-color:rgb(35, 38, 41);\n" \
                   "border-radius:10;\n" \
                   "border: 5px solid rgb(35, 38, 41);\n" \
                   "}\n" \
                   "QHeaderView{\n" \
                   "background-color:rgb(35, 38, 41);\n" \
                   "}\n" \
                   "QHeaderView::section{\n" \
                   "background-color:rgb(35, 38, 41);\n" \
                   "color:rgb(29, 233, 182);\n" \
                   "}"

# 记录表格的滚动条样式
rtScrollBarStyle = "QScrollBar:vertical{\n" \
                   "    border-radius:4px;\n" \
                   "    background:rgb(29, 233, 182);\n" \
                   "    padding-top:25px;\n" \
                   "    padding-bottom:25px;\n" \
                   "    padding-left:2px;\n" \
                   "    padding-right:2px;\n" \
                   "}\n" \
                   "QScrollBar::handle:vertical\n" \
                   "{\n" \
                   "    background:white;\n" \
                   "    border-radius:4px;\n" \
                   "}\n" \
                   "QScrollBar::handle:vertical:hover\n" \
                   "{\n" \
                   "    background:rgb(200, 200, 200);\n" \
                   "    border-radius:4px;\n" \
                   "}\n" \
                   "QScrollBar::handle:vertical:pressed\n" \
                   "{\n" \
                   "    background:rgb(150, 150, 150);\n" \
                   "    border-radius:4px;\n" \
                   "}\n" \
                   "QScrollBar::add-page:vertical\n" \
                   "{\n" \
                   "    background:rgb(79, 91, 98);\n" \
                   "}\n" \
                   "QScrollBar::sub-page:vertical\n" \
                   "{\n" \
                   "    background:rgb(29, 233, 182); \n" \
                   "}"

# 内存情况表格样式
memoryTableStyle = "QTableWidget{\n" \
                   "background-color: rgb(35, 38, 41);\n" \
                   "color:rgb(225, 172, 14);\n" \
                   "gridline-color:rgb(35, 38, 41);\n" \
                   "border-radius:10;\n" \
                   "border: 5px solid rgb(35, 38, 41);\n" \
                   "}\n" \
                   "QHeaderView{\n" \
                   "background-color:rgb(35, 38, 41);\n" \
                   "}\n" \
                   "QHeaderView::section{\n" \
                   "background-color:rgb(35, 38, 41);\n" \
                   "color:rgb(225, 172, 14);\n" \
                   "}"

# 当前算法标签样式
algorithmLabelStyle = "QLabel{\n" \
                      "background-color: rgb(35, 38, 41);\n" \
                      "color: rgb(26, 146, 166);\n" \
                      "border-radius: 5px;\n" \
                      "border:2px solid rgb(26, 146, 166);\n" \
                      "}"

# 选择算法按钮样式
algorithmButtonStyle = "QPushButton{\n" \
                       "background-color: rgb(79, 91, 98);\n" \
                       "color: rgb(29, 233, 182);\n" \
                       "border-radius: 20px;\n" \
                       "border: 2px solid rgb(29, 233, 182);\n" \
                       "}\n" \
                       "QPushButton:hover{\n" \
                       "background-color: rgb(77, 199, 176);\n" \
                       "}\n" \
                       "QPushButton:pressed{\n" \
                       "background-color: rgb(49, 54, 59);\n" \
                       "color: rgb(29, 233, 182);\n" \
                       "}\n" \
                       "QPushButton:disabled{\n" \
                       "background-color: rgb(79, 91, 98);\n" \
                       "color: rgb(29, 233, 182);\n" \
                       "border-radius: 20px;\n" \
                       "border: 2px solid rgb(29, 233, 182);\n" \
                       "}"

# 运行键样式
runButtonStyle = "QPushButton{\n" \
                 "background-color: rgb(225, 172, 14);\n" \
                 "color: rgb(35, 38, 41);\n" \
                 "border-radius: 20px;\n" \
                 "}\n" \
                 "QPushButton:hover{\n" \
                 "background-color: yellow;\n" \
                 "}\n" \
                 "QPushButton:pressed{\n" \
                 "background-color: rgb(49, 54, 59);\n" \
                 "color: rgb(29, 233, 182);\n" \
                 "}\n" \
                 "QPushButton:disabled{\n" \
                 "background-color: rgb(200, 200, 200);\n" \
                 "color: white;\n" \
                 "}"

# 复位键样式
resetButtonStyle = "QPushButton{\n" \
                   "background-color: rgb(204, 52, 67);\n" \
                   "color: rgb(35, 38, 41);\n" \
                   "border-radius: 20px;\n" \
                   "}\n" \
                   "QPushButton:hover{\n" \
                   "background-color: red;\n" \
                   "}\n" \
                   "QPushButton:pressed{\n" \
                   "background-color: rgb(49, 54, 59);\n" \
                   "color: rgb(29, 233, 182);\n" \
                   "}\n" \
                   "QPushButton:disabled{\n" \
                   "background-color: rgb(200, 200, 200);\n" \
                   "color: white;\n" \
                   "}"

# 缺页标签样式
missLabelStyle = "QLabel{\n" \
                 "background-color: rgb(35, 38, 41);\n" \
                 "color: rgb(204, 52, 67);\n" \
                 "border-radius: 5px;\n" \
                 "border:2px solid rgb(204, 52, 67);\n" \
                 "}"

# 内存情况表格标签样式
memoryLabelStyle = "QLabel{\n" \
                   "background-color: rgb(35, 38, 41);\n" \
                   "color: rgb(225, 172, 14);\n" \
                   "border-radius: 5px;\n" \
                   "border:2px solid rgb(225, 172, 14);\n" \
                   "}"

# 记录表格标签样式
recordLabelStyle = "QLabel{\n" \
                   "background-color: rgb(35, 38, 41);\n" \
                   "color: rgb(29, 233, 182);\n" \
                   "border-radius: 5px;\n" \
                   "border:2px solid rgb(29, 233, 182);\n" \
                   "}"

# 信息页面背景样式
infoBackgroundStyle = "QLabel{\n" \
                      "background-color: rgb(35, 38, 41);\n" \
                      "border-radius: 5px;\n" \
                      "}"

# 信息页面标签样式
infoLabelStyle = "QLabel{\n" \
                 "background-color: rgb(35, 38, 41);\n" \
                 "color: rgb(79, 91, 98);\n" \
                 "border-radius: 5px;\n" \
                 "border:2px solid rgb(79, 91, 98);\n" \
                 "}"

# 当前指令标签样式
currentCmdLabelStyle = "QLabel{\n" \
                       "background-color: rgb(35, 38, 41);\n" \
                       "color: rgb(29, 233, 182);\n" \
                       "border-radius: 5px;\n" \
                       "border:2px solid rgb(29, 233, 182);\n" \
                       "}"

# 剩余指令标签样式
leftCmdLabelStyle = "QLabel{\n" \
                    "background-color: rgb(35, 38, 41);\n" \
                    "color: rgb(225, 172, 14);\n" \
                    "border-radius: 5px;\n" \
                    "border:2px solid rgb(225, 172, 14);\n" \
                    "}"

# 状态标签样式
stateLabelStyle = "QLabel{\n" \
                  "background-color: rgb(35, 38, 41);\n" \
                  "color: rgb(79, 91, 98);\n" \
                  "border-radius: 5px;\n" \
                  "border:2px solid rgb(79, 91, 98);\n" \
                  "}"

# 控制台背景样式
consoleBackgroundStyle = "QLabel{\n" \
                         "background-color: rgb(35, 38, 41);\n" \
                         "border-radius: 5px;\n" \
                         "}"

# 控制台标签样式
consoleLabelStyle = "QLabel{\n" \
                    "background-color: rgb(35, 38, 41);\n" \
                    "color: rgb(119, 137, 147);\n" \
                    "border-radius: 5px;\n" \
                    "border:2px solid rgb(119, 137, 147);\n" \
                    "}"

# 时间间隔控制滑动条样式
intervalEditSliderStyle = "QSlider{\n" \
                          "border-radius:5px;\n" \
                          "}\n" \
                          "QSlider::handle{\n" \
                          "background-color: white;\n" \
                          "border-radius:5px;\n" \
                          "}\n" \
                          "QSlider::handle:hover{\n" \
                          "background-color: rgb(200, 200, 200);\n" \
                          "border-radius:5px;\n" \
                          "}\n" \
                          "QSlider::handle:pressed{\n" \
                          "background-color: rgb(150, 150, 150);\n" \
                          "border-radius:5px;\n" \
                          "}\n" \
                          "QSlider::sub-page{\n" \
                          "background-color: rgb(79, 91, 98);\n" \
                          "border-radius:5px;\n" \
                          "}\n" \
                          "QSlider::add-page{\n" \
                          "background-color: rgb(26, 146, 166);\n" \
                          "border-radius:5px;\n" \
                          "}"

# 时间间隔标签样式
intervalLabelStyle = "QLabel{\n" \
                     "background-color: rgb(35, 38, 41);\n" \
                     "color: rgb(26, 146, 166);\n" \
                     "border-radius: 5px;\n" \
                     "border:2px solid rgb(26, 146, 166);\n" \
                     "}"


class MyWindow(QWidget):
    def __init__(self):
        super(MyWindow, self).__init__()
        self.init_interface()

    def init_interface(self):
        # 1.给主窗口命名并设置大小
        self.setObjectName("MainWindow")
        self.resize(1348, 884)
        self.setStyleSheet("QWidget{\n"
                           "background-color:rgb(49, 54, 59);\n"
                           "}")

        # 2.设置记录表格
        self.RecordTable = QtWidgets.QTableWidget(self)
        self.RecordTable.setGeometry(QtCore.QRect(20, 330, 501, 491))
        self.RecordTable.setAutoFillBackground(False)
        self.RecordTable.setStyleSheet(recordTableStyle)
        self.RecordTable.setSizeAdjustPolicy(QtWidgets.QAbstractScrollArea.AdjustIgnored)
        self.RecordTable.setWordWrap(True)
        self.RecordTable.setCornerButtonEnabled(True)
        self.RecordTable.setRowCount(320)
        self.RecordTable.setColumnCount(3)
        self.RecordTable.setObjectName("RecordTable")
        self.RecordTable.verticalHeader().setVisible(False)
        self.RecordTable.setEditTriggers(QAbstractItemView.NoEditTriggers)
        self.RecordTable.setSelectionMode(QAbstractItemView.NoSelection)

        # 3.设置记录表格表头和第一行
        for i in range(3):
            item = QtWidgets.QTableWidgetItem()
            item.setBackground(QtGui.QColor(35, 38, 41))
            brush = QtGui.QBrush(QtGui.QColor(29, 233, 182))
            brush.setStyle(QtCore.Qt.NoBrush)
            item.setForeground(brush)
            self.RecordTable.setHorizontalHeaderItem(i, item)

        self.RecordTable.horizontalHeaderItem(0).setText("序号")
        self.RecordTable.horizontalHeaderItem(1).setText("指令地址")
        self.RecordTable.horizontalHeaderItem(2).setText("是否命中")

        for i in range(3):
            item = QtWidgets.QTableWidgetItem()
            self.RecordTable.setItem(0, i, item)
            self.RecordTable.item(0, i).setText("NULL")

        # 4.设置记录表格滚动条
        self.RtScrollBar = QtWidgets.QScrollBar()
        self.RtScrollBar.setStyleSheet(rtScrollBarStyle)
        self.RtScrollBar.setObjectName("RtScrollBar")
        self.RecordTable.setVerticalScrollBar(self.RtScrollBar)

        # 5.设置内存情况表格
        self.MemoryTable = QtWidgets.QTableWidget(self)
        self.MemoryTable.setGeometry(QtCore.QRect(30, 50, 461, 241))
        font = QtGui.QFont()
        font.setPointSize(9)
        self.MemoryTable.setFont(font)
        self.MemoryTable.setStyleSheet(memoryTableStyle)
        self.MemoryTable.setRowCount(4)
        self.MemoryTable.setObjectName("MemoryTable")
        self.MemoryTable.setColumnCount(3)
        self.MemoryTable.horizontalHeader().setVisible(True)
        self.MemoryTable.horizontalHeader().setCascadingSectionResizes(False)
        self.MemoryTable.verticalHeader().setVisible(False)
        self.MemoryTable.setEditTriggers(QAbstractItemView.NoEditTriggers)
        self.MemoryTable.setSelectionMode(QAbstractItemView.NoSelection)

        # 6.设置内存情况表格表头和第一行
        for i in range(3):
            item = QtWidgets.QTableWidgetItem()
            self.MemoryTable.setHorizontalHeaderItem(i, item)

        self.MemoryTable.horizontalHeaderItem(0).setText("块编号")
        self.MemoryTable.horizontalHeaderItem(1).setText("页编号")
        self.MemoryTable.horizontalHeaderItem(2).setText("地址范围")

        for i in range(4):
            for j in range(3):
                item = QtWidgets.QTableWidgetItem()
                self.MemoryTable.setItem(i, j, item)

                if j == 0:
                    self.MemoryTable.item(i, j).setText(f"{i}")
                else:
                    self.MemoryTable.item(i, j).setText("NULL")

        # 7.设置当前算法标签
        self.AlgorithmLabel = QtWidgets.QLabel(self)
        self.AlgorithmLabel.setGeometry(QtCore.QRect(670, 60, 121, 81))
        self.AlgorithmLabel.setFont(fontBig)
        self.AlgorithmLabel.setStyleSheet(algorithmLabelStyle)
        self.AlgorithmLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.AlgorithmLabel.setObjectName("AlgorithmLabel")
        self.AlgorithmLabel.setText("置换算法")

        self.AlgorithmDataLabel = QtWidgets.QLabel(self)
        self.AlgorithmDataLabel.setGeometry(QtCore.QRect(790, 60, 121, 81))
        self.AlgorithmDataLabel.setFont(fontBig)
        self.AlgorithmDataLabel.setStyleSheet(algorithmLabelStyle)
        self.AlgorithmDataLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.AlgorithmDataLabel.setObjectName("AlgorithmDataLabel")
        self.AlgorithmDataLabel.setText("FIFO")

        # 8.设置FIFO算法选择键
        self.FIFO = QtWidgets.QPushButton(self)
        self.FIFO.setGeometry(QtCore.QRect(730, 450, 171, 61))
        self.FIFO.setFont(fontBig)
        self.FIFO.setStyleSheet(algorithmButtonStyle)
        self.FIFO.setObjectName("FIFO")
        self.FIFO.clicked.connect(partial(select_algorithm, "FIFO"))
        self.FIFO.setText("FIFO")

        # 9.设置LRU算法选择键
        self.LRU = QtWidgets.QPushButton(self)
        self.LRU.setGeometry(QtCore.QRect(730, 540, 171, 61))
        self.LRU.setFont(fontBig)
        self.LRU.setStyleSheet(algorithmButtonStyle)
        self.LRU.setObjectName("LRU")
        self.LRU.clicked.connect(partial(select_algorithm, "LRU"))
        self.LRU.setText("LRU")

        # 10.设置OPT算法选择键
        self.OPT = QtWidgets.QPushButton(self)
        self.OPT.setGeometry(QtCore.QRect(980, 540, 171, 61))
        self.OPT.setFont(fontBig)
        self.OPT.setStyleSheet(algorithmButtonStyle)
        self.OPT.setObjectName("OPT")
        self.OPT.clicked.connect(partial(select_algorithm, "OPT"))
        self.OPT.setText("OPT")

        # 11.设置LFU算法选择键
        self.LFU = QtWidgets.QPushButton(self)
        self.LFU.setGeometry(QtCore.QRect(980, 450, 171, 61))
        self.LFU.setFont(fontBig)
        self.LFU.setStyleSheet(algorithmButtonStyle)
        self.LFU.setObjectName("LFU")
        self.LFU.clicked.connect(partial(select_algorithm, "LFU"))
        self.LFU.setText("LFU")

        # 12.设置运行键
        self.RunButton = QtWidgets.QPushButton(self)
        self.RunButton.setGeometry(QtCore.QRect(730, 630, 171, 101))
        self.RunButton.setFont(fontBig)
        self.RunButton.setStyleSheet(runButtonStyle)
        self.RunButton.setObjectName("RunButton")
        self.RunButton.clicked.connect(trigger_execute)
        self.RunButton.setText("运行")

        # 13.设置复位键
        self.ResetButton = QtWidgets.QPushButton(self)
        self.ResetButton.setGeometry(QtCore.QRect(980, 630, 171, 101))
        self.ResetButton.setFont(fontBig)
        self.ResetButton.setStyleSheet(resetButtonStyle)
        self.ResetButton.setObjectName("ResetButton")
        self.ResetButton.clicked.connect(trigger_reset)
        self.ResetButton.setEnabled(False)
        self.ResetButton.setText("复位")

        # 14.设置缺页标签
        self.MissCountLabel = QtWidgets.QLabel(self)
        self.MissCountLabel.setGeometry(QtCore.QRect(670, 200, 121, 81))
        self.MissCountLabel.setFont(fontBig)
        self.MissCountLabel.setStyleSheet(missLabelStyle)
        self.MissCountLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.MissCountLabel.setObjectName("MissCountLabel")
        self.MissCountLabel.setText("缺页数")

        self.MissRateLabel = QtWidgets.QLabel(self)
        self.MissRateLabel.setGeometry(QtCore.QRect(670, 280, 121, 81))
        self.MissRateLabel.setFont(fontBig)
        self.MissRateLabel.setStyleSheet(missLabelStyle)
        self.MissRateLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.MissRateLabel.setObjectName("MissRateLabel")
        self.MissRateLabel.setText("缺页率")

        self.MissCountDataLabel = QtWidgets.QLabel(self)
        self.MissCountDataLabel.setGeometry(QtCore.QRect(790, 200, 121, 81))
        self.MissCountDataLabel.setFont(fontBig)
        self.MissCountDataLabel.setStyleSheet(missLabelStyle)
        self.MissCountDataLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.MissCountDataLabel.setObjectName("MissCountDataLabel")
        self.MissCountDataLabel.setText("0")

        self.MissRateDataLabel = QtWidgets.QLabel(self)
        self.MissRateDataLabel.setGeometry(QtCore.QRect(790, 280, 121, 81))
        self.MissRateDataLabel.setFont(fontBig)
        self.MissRateDataLabel.setStyleSheet(missLabelStyle)
        self.MissRateDataLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.MissRateDataLabel.setObjectName("MissRateDataLabel")
        self.MissRateDataLabel.setText("NULL")

        # 15.设置内存情况表格的标签
        self.MemoryLabel = QtWidgets.QLabel(self)
        self.MemoryLabel.setGeometry(QtCore.QRect(490, 50, 61, 241))
        self.MemoryLabel.setFont(fontBig)
        self.MemoryLabel.setStyleSheet(memoryLabelStyle)
        self.MemoryLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.MemoryLabel.setWordWrap(True)
        self.MemoryLabel.setObjectName("MemoryLabel")
        self.MemoryLabel.setText("<html><head/><body><p>内</p><p>存</p><p>情</p><p>况</p></body></html>")

        # 16.设置记录表格的标签
        self.RecordLabel = QtWidgets.QLabel(self)
        self.RecordLabel.setGeometry(QtCore.QRect(520, 330, 61, 491))
        self.RecordLabel.setFont(fontBig)
        self.RecordLabel.setStyleSheet(recordLabelStyle)
        self.RecordLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.RecordLabel.setWordWrap(True)
        self.RecordLabel.setObjectName("RecordLabel")
        self.RecordLabel.setText("<html><head/><body><p>访</p><p>问</p><p>指</p><p>令</p><p>记</p><p>录</p></body></html>")

        # 17.设置信息页面背景
        self.InfoBackground = QtWidgets.QLabel(self)
        self.InfoBackground.setGeometry(QtCore.QRect(630, 20, 611, 391))
        self.InfoBackground.setStyleSheet(infoBackgroundStyle)
        self.InfoBackground.setText("")
        self.InfoBackground.setObjectName("InfoBackground")

        # 18.设置信息页面标签
        self.InfoLabel = QtWidgets.QLabel(self)
        self.InfoLabel.setGeometry(QtCore.QRect(1240, 20, 61, 391))
        self.InfoLabel.setFont(fontBig)
        self.InfoLabel.setStyleSheet(infoLabelStyle)
        self.InfoLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.InfoLabel.setObjectName("InfoLabel")
        self.InfoLabel.setText("<html><head/><body><p>信</p><p>息</p><p>页</p><p>面</p></body></html>")

        # 19.设置当前指令标签
        self.CurrentCmdLabel = QtWidgets.QLabel(self)
        self.CurrentCmdLabel.setGeometry(QtCore.QRect(950, 60, 121, 81))
        self.CurrentCmdLabel.setFont(fontBig)
        self.CurrentCmdLabel.setStyleSheet(currentCmdLabelStyle)
        self.CurrentCmdLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.CurrentCmdLabel.setObjectName("CurrentCmdLabel")
        self.CurrentCmdLabel.setText("<html><head/><body><p>当前指令</p><p>物理地址</p></body></html>")

        self.CurrentCmdDataLabel = QtWidgets.QLabel(self)
        self.CurrentCmdDataLabel.setGeometry(QtCore.QRect(1070, 60, 121, 81))
        self.CurrentCmdDataLabel.setFont(fontBig)
        self.CurrentCmdDataLabel.setStyleSheet(currentCmdLabelStyle)
        self.CurrentCmdDataLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.CurrentCmdDataLabel.setObjectName("CurrentCmdDataLabel")
        self.CurrentCmdDataLabel.setText("NULL")

        # 20.设置剩余指令标签
        self.LeftCmdLabel = QtWidgets.QLabel(self)
        self.LeftCmdLabel.setGeometry(QtCore.QRect(950, 170, 121, 81))
        self.LeftCmdLabel.setFont(fontBig)
        self.LeftCmdLabel.setStyleSheet(leftCmdLabelStyle)
        self.LeftCmdLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.LeftCmdLabel.setObjectName("LeftCmdLabel")
        self.LeftCmdLabel.setText("剩余指令")

        self.LeftCmdDataLabel = QtWidgets.QLabel(self)
        self.LeftCmdDataLabel.setGeometry(QtCore.QRect(1070, 170, 121, 81))
        self.LeftCmdDataLabel.setFont(fontBig)
        self.LeftCmdDataLabel.setStyleSheet(leftCmdLabelStyle)
        self.LeftCmdDataLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.LeftCmdDataLabel.setObjectName("LeftCmdDataLabel")
        self.LeftCmdDataLabel.setText("NULL")

        # 21.设置状态标签
        self.StateLabel = QtWidgets.QLabel(self)
        self.StateLabel.setGeometry(QtCore.QRect(950, 280, 121, 81))
        self.StateLabel.setFont(fontBig)
        self.StateLabel.setStyleSheet(stateLabelStyle)
        self.StateLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.StateLabel.setObjectName("StateLabel")
        self.StateLabel.setText("状态")

        self.StateDataLabel = QtWidgets.QLabel(self)
        self.StateDataLabel.setGeometry(QtCore.QRect(1070, 280, 121, 81))
        self.StateDataLabel.setFont(fontBig)
        self.StateDataLabel.setStyleSheet(stateLabelStyle)
        self.StateDataLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.StateDataLabel.setObjectName("StateDataLabel")
        self.StateDataLabel.setText("空闲")

        # 22.设置控制台背景
        self.ConsoleBackground = QtWidgets.QLabel(self)
        self.ConsoleBackground.setGeometry(QtCore.QRect(630, 430, 611, 391))
        self.ConsoleBackground.setStyleSheet(consoleBackgroundStyle)
        self.ConsoleBackground.setText("")
        self.ConsoleBackground.setObjectName("ConsoleBackground")

        # 23.设置控制台标签
        self.ConsoleLabel = QtWidgets.QLabel(self)
        self.ConsoleLabel.setGeometry(QtCore.QRect(1240, 430, 61, 391))
        self.ConsoleLabel.setFont(fontBig)
        self.ConsoleLabel.setStyleSheet(consoleLabelStyle)
        self.ConsoleLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.ConsoleLabel.setObjectName("ConsoleLabel")
        self.ConsoleLabel.setText("<html><head/><body><p>控</p><p>制</p><p>台</p></body></html>")

        # 24.设置时间间隔控制滑动条
        self.IntervalEditSlider = QtWidgets.QSlider(self)
        self.IntervalEditSlider.setGeometry(QtCore.QRect(830, 770, 221, 16))
        self.IntervalEditSlider.setStyleSheet(intervalEditSliderStyle)
        self.IntervalEditSlider.setMinimum(0)
        self.IntervalEditSlider.setMaximum(100)
        self.IntervalEditSlider.setSingleStep(1)
        self.IntervalEditSlider.setOrientation(QtCore.Qt.Horizontal)
        self.IntervalEditSlider.setObjectName("IntervalEditSlider")

        # 25.设置时间间隔标签
        self.IntervalLabel = QtWidgets.QLabel(self)
        self.IntervalLabel.setGeometry(QtCore.QRect(690, 750, 121, 51))
        self.IntervalLabel.setFont(fontBig)
        self.IntervalLabel.setStyleSheet(intervalLabelStyle)
        self.IntervalLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.IntervalLabel.setObjectName("IntervalLabel")
        self.IntervalLabel.setText("时间间隔")

        self.IntervalDataLabel = QtWidgets.QLabel(self)
        self.IntervalDataLabel.setGeometry(QtCore.QRect(1070, 750, 121, 51))
        self.IntervalDataLabel.setFont(fontBig)
        self.IntervalDataLabel.setStyleSheet(intervalLabelStyle)
        self.IntervalDataLabel.setAlignment(QtCore.Qt.AlignCenter)
        self.IntervalDataLabel.setObjectName("IntervalDataLabel")
        self.IntervalDataLabel.setText("无间隔")

        # 26.将组件上移一层，以达到有背景的效果
        self.ConsoleBackground.raise_()
        self.InfoBackground.raise_()
        self.RecordTable.raise_()
        self.MemoryTable.raise_()
        self.AlgorithmLabel.raise_()
        self.FIFO.raise_()
        self.LRU.raise_()
        self.OPT.raise_()
        self.RunButton.raise_()
        self.ResetButton.raise_()
        self.MissCountLabel.raise_()
        self.MissRateLabel.raise_()
        self.MissCountDataLabel.raise_()
        self.MissRateDataLabel.raise_()
        self.AlgorithmDataLabel.raise_()
        self.MemoryLabel.raise_()
        self.RecordLabel.raise_()
        self.InfoLabel.raise_()
        self.CurrentCmdLabel.raise_()
        self.CurrentCmdDataLabel.raise_()
        self.LeftCmdLabel.raise_()
        self.StateLabel.raise_()
        self.LeftCmdDataLabel.raise_()
        self.ConsoleLabel.raise_()
        self.LFU.raise_()
        self.StateDataLabel.raise_()
        self.IntervalEditSlider.raise_()
        self.IntervalLabel.raise_()
        self.IntervalDataLabel.raise_()

        # 27.设置窗口背景并显示窗口
        self.setWindowTitle("Memory-Management Simulator v1.0")
        self.show()


# 专门处理界面更新的线程，在run里没有耗时操作
class UiThread(QThread):
    signal_memory = pyqtSignal(int)  # 更新内存情况表格的信号对象
    signal_record = pyqtSignal(int, int, bool)  # 更新记录表格的信号对象
    signal_miss = pyqtSignal()  # 更新缺页情况标签的信号对象
    signal_reset = pyqtSignal()  # 使表格复位的信号对象
    ins_num = -1             # 更新记录表格用的临时变量：指令序号
    ins_address = -1         # 更新记录表格用的临时变量：指令地址
    ins_hit = False          # 更新记录表格用的临时变量：是否命中
    new_page = -1            # 更新内存情况表格用的临时变量：新页编号
    physical_address = -1    # 更新当前指令标签用的临时变量：物理地址
    current_interval = 0.05  # 更新时间间隔用的临时变量：当前时间间隔
    run_button_enabled = True     # 切换按键状态用的临时变量：运行键是否有效
    reset_button_enabled = False  # 切换按键状态用的临时变量：复位键是否有效

    def __init__(self):
        super(UiThread, self).__init__()

        # 各信号与对应的槽函数连接
        self.signal_memory.connect(update_memory)
        self.signal_record.connect(update_record)
        self.signal_miss.connect(update_miss_info)
        self.signal_reset.connect(reset_memory)
        self.signal_reset.connect(reset_record)

    def run(self):
        global update, lupdate, reset_ui, miss_count, reset_data, mainWindow, record, interval, memory, miss

        while 1:  # 线程运行过程处在永真循环中
            latest_interval = mainWindow.IntervalEditSlider.value()

            # 一、若时间间隔发生改变
            if latest_interval != self.current_interval:
                self.current_interval = latest_interval  # 更新临时变量
                interval = latest_interval  # 更新时间间隔
                mainWindow.IntervalDataLabel.setText(f"{latest_interval / 100}s" if interval != 0 else "无间隔")  # 改标签

            # 二、若发生缺页
            if miss:
                lmiss.lock()  # -------------------------------------  互斥锁上锁  -------------------------------------
                self.signal_memory.emit(memory.index(self.new_page))  # 更新内存情况表格
                miss = False  # 更新结束，关闭信号变量
                lmiss.unlock()  # ------------------------------------  互斥锁解锁  ------------------------------------

            # 三、若需要更新信息
            if update:
                lupdate.lock()  # ------------------------------------  互斥锁上锁  ------------------------------------
                self.signal_record.emit(self.ins_num, self.ins_address, self.ins_hit)  # 更新表格
                self.signal_miss.emit()  # 更新缺页信息标签
                mainWindow.CurrentCmdDataLabel.setText(f"{self.physical_address}")  # 更新当前指令标签
                mainWindow.LeftCmdDataLabel.setText(f"{319 - self.ins_num}")  # 更新剩余指令标签
                QApplication.processEvents()  # 处理所有emit()请求，以达到实时更新

                time.sleep(interval / 100)  # 延时使得画面更为流畅
                update = False  # 更新结束，关闭信号变量
                lupdate.unlock()  # ------------------------------------ 互斥锁解锁 ------------------------------------

            # 四、若需要复位界面
            if reset_ui:
                lreset.lock()  # -------------------------------------  互斥锁上锁  ------------------------------------
                miss_count = 0
                self.signal_miss.emit()  # 复位缺页信息标签
                self.signal_reset.emit()  # 复位两个表格
                mainWindow.CurrentCmdDataLabel.setText("NULL")  # 复位当前指令标签
                mainWindow.LeftCmdDataLabel.setText("320")  # 复位剩余指令标签
                mainWindow.StateDataLabel.setText("空闲")  # 复位状态标签
                mainWindow.RunButton.setEnabled(True)  # 复位运行键
                mainWindow.ResetButton.setEnabled(False)  # 复位复位键
                self.run_button_enabled = True  # 临时变量更新，运行键设为有效
                self.reset_button_enabled = False  # 临时变量更新，复位键设为无效
                QApplication.processEvents()  # 处理所有emit()请求，以达到实时更新
                reset_ui = False   # 复位ui结束，关闭信号变量
                reset_data = True  # 打开信号变量，通知复位数据开始
                lreset.unlock()  # ------------------------------------- 互斥锁解锁 ------------------------------------

            # 五、若处在刚开始运行瞬间
            if execute and self.run_button_enabled:
                mainWindow.RunButton.setEnabled(False)  # 运行键设为无效
                mainWindow.FIFO.setEnabled(False)  # 算法选择键均设为无效
                mainWindow.LRU.setEnabled(False)
                mainWindow.LFU.setEnabled(False)
                mainWindow.OPT.setEnabled(False)
                mainWindow.StateDataLabel.setText("运行")  # 更新状态标签
                self.run_button_enabled = False  # 临时变量更新，运行键设为无效

            # 六、若处在运行完毕的瞬间
            elif not execute and not self.reset_button_enabled and not reset_ui and not reset_data and len(record) != 0:
                mainWindow.ResetButton.setEnabled(True)  # 复位键设为有效
                mainWindow.FIFO.setEnabled(True)  # 算法选择键均设为有效
                mainWindow.LRU.setEnabled(True)
                mainWindow.LFU.setEnabled(True)
                mainWindow.OPT.setEnabled(True)
                mainWindow.StateDataLabel.setText("停止")  # 更新状态标签
                self.reset_button_enabled = True  # 临时变量更新，复位键设为有效

    def set_record_info(self, num, address, hit):  # 更新记录表格时，将临时变量也更新，方便传送到槽函数
        self.ins_num = num
        self.ins_address = address
        self.ins_hit = hit

    def set_physical_address(self, physical_address):
        self.physical_address = physical_address

    def set_new_page(self, new_page_num):  # 更新内存情况表格时，将临时变量也更新，方便传送到槽函数
        self.new_page = new_page_num


# 专门处理指令序列的线程，直接在run里执行耗时的对数据的操作
class DataThread(QThread):
    def __init__(self):
        super(DataThread, self).__init__()

    def run(self):
        global execute, lexecute, update, reset_data, miss_count, miss, miss_rate

        while 1:
            # 一、若需要开始处理指令序列
            if execute:
                lexecute.lock()  # ------------------------------------  互斥锁上锁  -----------------------------------
                address = random.randint(0, 319)  # 初始随机地址
                print("#--------------------------------------------------------------------------------------------#\n"
                      "start processing...")  # 打印开始处理的标志

                # 1.若选择算法为OPT，则先取得指令序列和页号序列
                if algorithm == "OPT":
                    for i in range(320):
                        address = rand_ins(address)  # 将前一地址输入，获得当前地址
                        visited_ins.add(address)     # 将当前地址加入集合，防止获取重复地址
                        ins_order.append(address)    # 记录当前地址
                        page_order.append(int((address - address % 10) / 10))  # 记录当前页号

                    visited_ins.clear()  # 预获取地址结束，集合清空

                # 2.处理指令
                for i in range(320):
                    # 若选择算法为OPT，直接从记录中取地址和页号；否则随机生成地址并计算对应页号
                    address = rand_ins(address) if algorithm != "OPT" else ins_order[i]
                    page_num = int((address - address % 10) / 10) if algorithm != "OPT" else page_order[i]
                    hit = False  # 是否命中的标志

                    for page in memory:  # 遍历内存中的4个块，看是否命中
                        if page == page_num:
                            hit = True

                    if not hit:  # 若不命中，则进行调块
                        if algorithm == "FIFO":
                            FIFO(page_num)
                        elif algorithm == "LRU":
                            LRU(page_num)
                        elif algorithm == "OPT":
                            OPT(i, page_num)
                        elif algorithm == "LFU":
                            LFU(page_num)

                        miss_count += 1  # 缺页数+1

                    physical_address = memory.index(page_num) * 10 + address % 10  # 计算物理地址
                    visit(i, address, physical_address, hit, page_num)  # 访问该指令

                    if not hit:
                        lmiss.lock()  # ----------------------  互斥锁上锁  ----------------------
                        ui_thread.set_new_page(page_num)  # 改变UiThread的临时变量
                        miss = True  # 打开信号变量，通知更新内存情况表格开始
                        lmiss.unlock()  # ---------------------  互斥锁解锁  ---------------------

                    lupdate.lock()  # -----------------------  互斥锁上锁  -----------------------
                    ui_thread.set_record_info(i, address, hit)  # 改变UiThread的临时变量
                    ui_thread.set_physical_address(physical_address)    # 改变UiThread的临时变量
                    update = True  # 打开信号变量，通知更新界面开始
                    lupdate.unlock()  # ----------------------- 互斥锁解锁 -----------------------

                    while miss or update:  # 若界面正在更新，则阻塞指令处理的进行，以免处理太快导致界面或数据不同步
                        pass

                # 3.打印本次运行信息：置换算法、缺页数、缺页率
                print("#--------------------------------------------------------------------------------------------#\n"
                      "process success\n"
                      f"algorithm:{algorithm}\n"
                      f"miss_count:{miss_count}\n"
                      f"miss_rate:{round(miss_count / 3.2, 2)}%\n"
                      "press 复位 to continue next round\n"
                      "#--------------------------------------------------------------------------------------------#\n"
                      )

                execute = False  # 指令处理结束，关闭信号变量
                lexecute.unlock()  # ------------------------------------ 互斥锁解锁 -----------------------------------

            # 二、若需要复位数据
            if reset_data:
                lreset.lock()  # -------------------------------------  互斥锁上锁  ------------------------------------
                record.clear()  # 指令访问记录清空

                for i in range(4):  # 内存中块的情况重置
                    memory[i] = -1

                visited_ins.clear()  # 访问过的指令集合清空

                for i in range(queue.qsize()):  # 清空FIFO使用的队列
                    queue.get()

                for i in range(32):  # 清空LFU使用的页面访问次数记录数组
                    page_visit[i] = 0

                stack.clear()  # 清空LRU使用的特殊栈

                ins_order.clear()   # 清空OPT使用的地址序列
                page_order.clear()  # 清空OPT使用的页号序列

                reset_data = False  # 数据复位结束，关闭信号变量
                lreset.unlock()  # ------------------------------------  互斥锁解锁  -----------------------------------


# 选择算法
def select_algorithm(name):
    global algorithm

    mainWindow.AlgorithmDataLabel.setText(name)  # 更新当前算法标签
    algorithm = name  # 更新当前算法变量


# 打开信号变量，通知处理指令开始的函数
def trigger_execute():
    global execute

    lexecute.lock()  # -------------------  互斥锁上锁  -------------------
    execute = True  # 打开信号变量，通知处理指令开始
    lexecute.unlock()  # ------------------  互斥锁解锁  ------------------


# 打开信号变量，通知复位开始的函数
def trigger_reset():
    global reset_ui

    lreset.lock()  # --------------------  互斥锁上锁  --------------------
    reset_ui = True  # 打开信号变量，先通知处理界面开始
    lreset.unlock()  # -------------------  互斥锁解锁  -------------------


# 置换算法：FIFO
def FIFO(page_num):
    index = queue.qsize()  # 要替换的块的下标，初始为队列的长度，在内存仍有空闲块时可以取到第一个空闲位置

    if queue.qsize() == 4:  # 若无空闲块，则队头元素出队，并找到其下标
        index = memory.index(queue.get())

    queue.put(page_num)  # 新块入队
    memory[index] = page_num  # 新块替换内存中原来的块


# 置换算法：LRU
def LRU(page_num):
    if len(stack) < 4:  # 若仍有空闲块，直接向栈顶加新块，新块调入第一个空闲位置
        stack.append(page_num)
        memory[len(stack) - 1] = page_num
    else:  # 若无空闲块，则找到栈底元素对应的块，既修改栈底元素，也修改内存中对应的块
        index = memory.index(stack[0])
        stack[0] = page_num
        memory[index] = page_num


# 置换算法：OPT
def OPT(current, page_num):
    if memory.__contains__(-1):  # 若仍有空闲块，直接向栈顶加新块，新块调入第一个空闲位置
        memory[memory.index(-1)] = page_num
    else:  # 若无空闲块
        indexes = []  # 四个块的下一次访问的指令序号

        for i in range(4):  # 从当前指令往后，统计四个块的第一次出现序号
            if page_order[current:320].__contains__(memory[i]):  # 若能找到则记录序号
                index = page_order.index(memory[i], current, 320)
            else:  # 若不能找到则记为无限大
                index = 2147483647

            indexes.append(index)

        max_index = indexes.index(max(indexes))  # 取序号最大的块在memory中的下标，并进行取代
        memory[max_index] = page_num


# 置换算法：LFU
def LFU(page_num):
    if memory.__contains__(-1):  # 若仍有空闲块，直接向栈顶加新块，新块调入第一个空闲位置
        memory[memory.index(-1)] = page_num
    else:  # 若无空闲块
        visit_count = []  # 四个块的被访问次数

        for i in range(4):
            visit_count.append(page_visit[memory[i]])  # 从page_visit总数组中找到四个块的被访问次数

        max_index = visit_count.index(max(visit_count))  # 选出次数最多的块进行取代
        memory[max_index] = page_num


# 更新内存情况表格
def update_memory(update_pos):
    # 更新对应位置有信息的两个单元格
    mainWindow.MemoryTable.item(update_pos, 1).setText(f"{memory[update_pos]}")
    mainWindow.MemoryTable.item(update_pos, 2).setText(f"[{memory[update_pos] * 10}, {(memory[update_pos] + 1) * 10})")


# 更新记录表格
def update_record(num, address, hit):
    record.append([num, address, hit])  # 将当前记录加到数组中
    color = QtGui.QColor(26, 146, 166) if hit else QtGui.QColor(204, 52, 67)  # 根据本次访问是否命中而选择颜色

    if num >= 1:  # 第一行单元格的实例已经生成，只在第二行之后进行
        for i in range(3):  # 生成一行三个单元格实例，并加到记录表格中
            item = QtWidgets.QTableWidgetItem()
            mainWindow.RecordTable.setItem(num, i, item)

    # 设置单元格文本和颜色
    item = mainWindow.RecordTable.item(num, 0)
    item.setText(f"{num}")
    item.setForeground(color)
    item = mainWindow.RecordTable.item(num, 1)
    item.setText(f"{address}")
    item.setForeground(color)
    item = mainWindow.RecordTable.item(num, 2)
    item.setText(f"{hit}")
    item.setForeground(color)

    # 若当前记录超过9条，则滚动条自动拉到最新生成的那一行，完成实时更新的视觉效果
    if num > 9:
        mainWindow.RecordTable.scrollToItem(item)


# 复位内存情况表格
def reset_memory():
    for i in range(4):  # 将所有有信息单元格的内容设为NULL
        mainWindow.MemoryTable.item(i, 1).setText("NULL")
        mainWindow.MemoryTable.item(i, 2).setText("NULL")


# 复位记录表格
def reset_record():
    for i in range(3):  # 复位第一行的颜色和文本
        mainWindow.RecordTable.item(0, i).setForeground(QtGui.QColor(29, 233, 182))
        mainWindow.RecordTable.item(0, i).setText("NULL")

    for i in range(1, 320):  # 所有单元格除第一行外文本设置为空
        for j in range(3):
            mainWindow.RecordTable.item(i, j).setText("")

    mainWindow.RecordTable.scrollToTop()  # 滚动条拉到最上方


# 更新缺页情况标签
def update_miss_info():
    mainWindow.MissCountDataLabel.setText(f"{miss_count}")  # 更新缺页数

    # 更新缺页率
    if len(record) == 0 or miss_count == 0:
        mainWindow.MissRateDataLabel.setText("NULL")
    else:
        mainWindow.MissRateDataLabel.setText(f"{round(miss_count / len(record) * 100, 2)}%")


# 访问一条记录
def visit(ins_num, address, physical_address, hit, page_num):
    visited_ins.add(address)  # 将指令地址加入访问过的集合
    print(f"#{ins_num}----address:{address}    physical_address:{physical_address}    "
          f"hit:{hit}    memory:{memory}")  # 打印当前指令信息和内存情况

    if algorithm == "LRU":  # 选择算法为LRU时，维护特殊栈（将访问到的页抽出，放至栈顶）
        stack.remove(page_num)
        stack.append(page_num)
    elif algorithm == "LFU":  # 选择算法为LFU时，维护页被访问次数数组（对应页+1）
        page_visit[page_num] += 1


# 产生当前指令的地址
def rand_ins(last_address):
    result = last_address  # 最终结果，初始化为上一条指令的地址

    while True:
        rand = random.random()  # 随机产生小数

        if 0 <= rand < 0.7 and result < 319:  # 在上一条指令不是最后一条指令的前提下，70%的概率顺序执行
            result += 1
        elif 0.7 <= rand < 0.85 and result > 1:  # 在上一条指令不是前两条指令的前提下，15%的概率向前跳转
            result = random.randint(0, result - 1)
        elif 0.85 <= rand < 1 and result < 318:  # 在上一条指令不是倒数两条指令的前提下，15%的概率向后跳转
            result = random.randint(result + 1, 319)

        if result not in visited_ins:  # 若得出的指令确实没有被访问过，则得出结果；否则重新生成
            break

    return result


if __name__ == '__main__':
    app = QApplication(sys.argv)
    mainWindow = MyWindow()

    # 使用的替换算法
    algorithm = "FIFO"

    # 内存中块的情况
    memory = [-1, -1, -1, -1]

    # 指令访问情况
    record = []

    # 访问过指令地址的集合
    visited_ins = set([])

    # 特定替换算法使用的数据结构
    queue = Queue(4)  # FIFO使用的队列
    stack = []        # LRU使用的特殊栈
    ins_order = []    # OPT使用的已知的指令序列
    page_order = []   # OPT使用的页序列
    page_visit = []   # LFU使用的页面访问次数记录数组
    for i in range(32):
        page_visit.append(0)

    # 负责触发动作的信息变量
    miss = False        # 触发缺页时内存情况表格更新的标志
    update = False      # 触发信息更新的标志
    execute = False     # 触发处理指令序列的标志
    reset_ui = False    # 触发复位界面的标志
    reset_data = False  # 触发复位数据的标志

    # 处理指令序列时的时间间隔
    interval = 0

    # 显示页面
    mainWindow.show()

    # 更新界面的线程
    ui_thread = UiThread()
    ui_thread.start()

    # 处理指令序列的线程
    data_thread = DataThread()
    data_thread.start()

    # 互斥锁
    lmiss = QtCore.QMutex()      # 控制缺页时内存情况表格的更新（将其从lupdate中剥离，是因为只有发生缺页时才需要更新表格）
    lupdate = QtCore.QMutex()    # 控制信息更新
    lexecute = QtCore.QMutex()   # 控制指令序列处理
    lreset = QtCore.QMutex()     # 控制复位

    # 统计缺页数的变量
    miss_count = 0

    sys.exit(app.exec_())