### Ileus Extraction in X-Ray image using Air-fluid Level Filter and Haar-like Feature
# 공기 액체층 필터와 Haar-like feature를 이용한 X-Ray 영상에서의 장폐색 검출

## 요약
장폐색은 소화관의 일부가 완전히 막혀 음식물이 통과하는데 장애를 일으키는 질환이다. 장 폐색 진단은 X-Ray 검사로 가능하지만 육안으로 진단하는 방법은 검사자의 주관이 관여되어 진단 결과에 영향을 끼칠 수도 있기 때문에 장폐색에 대해 명확하고 객관적인 분석이 필요하다. 기존의 연구 방법에서는 소장 및 대장의 장폐색이 대략적이고 일부 영역만이 검출되는 문제점이 있다. 제안된 방법에서는 소화관에서 장폐색을 검출하기 위하여 장폐색의 중요한 특징인 공기액체층(Air-fluid level)영역을 필터로 정의하여 연속적인 Convolution 연산으로 구성한 Feature Map과 Haar-like feature로 구성한 Feature Map을 적용하여 종합적인 분석을 할 수 있는 Activation Map을 생성하고 모든 소화관 영역에 적용하여 픽셀 단위로 장폐색 영역을 추출한다. 제안된 방법을 119장의 장폐색 X-Ray 영상을 대상으로 실험한 결과, 정확도는 71.9%이고 정밀도는 81.4% 및 재현도는 84.8%으로 나타났다. 

## I. 서 론
장폐색은 식도에서 위, 십이지장, 소장, 대장에 이르는 소화관의 일부가 완전히 막혀 음식물이나 소화액이 통과하는데 장애를 일으키는 질환이나 증상을 말한다. 장폐색의 진단은 단순 복부 X-ray 검사로 가능하며, 장폐색이 발병한 환자의 X-ray영상을 관찰하면 장 내부의 물질이 배출되지 못하고 장이 팽창한다. 이 때, 장 내부로 액체와 공기가 모이고 공기는 위쪽으로, 액체는 아래쪽으로 중력에 의해 이동해 공기와 액체가 이루는 면이 자연스럽게 평평하게 되면서 공기 액체 층(Air-fluid level)이 관찰된다.[1] 이러한 특징은 영상학적 검사에서 중요한 진단의 키로서 작용할 수 있다. 검사자는 이러한 특징을 이용해 장폐색을 진단하는데, 육안으로 진단하는 방법은 검사자의 주관이 관여되기 때문에 진단결과에 영향을 미친다. 
기존의 방법[2]은 소장 폐색 영역에서 공기 액체 층이 뚜렷하게 나타나지 않는 X-ray 영상에서는 추출되지 않는 문제점이 있거나 대략적인 소장 및 대장의 장 폐색만 검출하여 정밀하게 분석할 수 없었다. 따라서 제안된 방법에서는 장폐색의 명암도가 급격히 변화한다는 특징과 공기 액체 층이 항상 수평을 이룬다는 두 가지의 형태학적 특징을 기반으로 대장 및 소장의 폐색 영역을 추출하는 방법을 제시한다. 제안된 방법에서는 공기액체층(Air-fluid level) 영역을 필터로 정의하여 연속적인 Convolution 연산으로 구성한 Feature Map과 Haar-like feature로 구성한 Feature Map에서 각각의 Feature map을 생성한다. Feature map은 haar-like feature의 Filtering으로 각각의 Feature map을 구하고 구한 두 개의 Feature map을 이용하여 Activation Map을 생성하여 픽셀 단위로 공기 액체 층의 특징을 분석하여 장폐색 영역을 검출한다. Activation map은 Feature Map 행렬에 활성화 함수를 적용한 결과이다. 제안된 장 폐색 검출 순서도는 그림 1과 같다.

![image](https://user-images.githubusercontent.com/56337609/174417770-05715c69-cc4a-46b1-a3bc-4f91e362743a.png)

## II. Feature Map with Air-fluid level Filter
공기액체층이 포함된 장폐색 X-Ray 영상은 그림 2와 같다.

![image](https://user-images.githubusercontent.com/56337609/174418233-cb3582c4-28db-42b8-9b03-8246b6c05d12.png)

### 2.1. Feature Detector
현재 전형적인 디스플레이는 하나의 픽셀에 256가지의 명암도를 결정한다. 장폐색의 핵심 특징인 공기 액체 층은 일정한 명암도를 가진다.  공기 액체 층의 명암도 값의 분석은 그림 3과 같다.

![image](https://user-images.githubusercontent.com/56337609/174418314-4d62faab-20bb-4a21-9c51-9a01ec91d9de.png)

그림 3에서 공기 액체 층의 분석하기 위해 119개의 장폐색 X-Ray 영상 중에 대표로 그림 2의 장폐색 영상의 7개의 공기 액체 층을 시각화한 것이다. 그림 3에서 세로축은 명암도 값이고 가로축은 공기 액체 층 영역의 수평 픽셀 수를 의미한다. 119개의 장폐색 X-Ray 영상에서 공기 액체 층은 654개지만 654개의 공기 액체 층을 시각화하는 경우에는 시각적으로 패턴을 파악하기 어렵기 때문에 7개의 공기 액체 층의 픽셀만 시각화한다.
공기 액체 층의 명암도에는 2가지 형태의  특징이 있다. 첫 번째는, 그림 3에서와 같이 공기 액체 층 영역의 수평 픽셀 수가 적은 경우에는  명암도 값이 높게 나타나고 수평 픽셀 수가 중간인 경우에는 명암도가 낮게 나타난다. 그리고   수평 픽셀 수가 많은 경우에는 명암도 값이 높게 나타나는 형태를 가진다. 이러한 원인은 공기 액체 층이 수평처럼 보이지만 실제로는  픽셀들이 돌출되어 있기 때문이다. 
두 번째 특징은 공기 액체 층의 명암도는 그림 3과 같이 명암도가 80에서 140까지의 분포되어 있다. 뼈 영역의 명암도는 150부터 200까지의 명암도를 가지고 장이 팽창한 어두운 영역은 50에서 100까지의 명암도를 가진다. 공기 액체 층은 명암도가 80에서 140까지의 분포되어 있어 뼈 영역의 명암도 분포와 장이 팽창한 어두운 영역의 명암도 분포에서 중간 형태의 분포를 가진다. 이러한 형태가 일반적인 공기 액체 층의 형태이다. 따라서 이와 같은 두 가지의 특징을 기반으로 필터를 구성하고 Feature Map을 생성한다.
그림 3에서  청색 라인, 노란색 라인, 초록색 라인, 회색 라인, 하늘색 라인 등은 공기 액체층의 명암도가 80에서 140까지 일정하게 나타났고 수평 픽셀 수가 적은 경우에는 명암도 값이 높게 나타나고 수평 픽셀 수가 중간인 경우에는 명암도가 낮게 나타난다. 그리고 수평 픽셀 수가 많은 경우에는 명암도 값이 높게 나타나는  일정한 형태를 가진다.
청색라인은 명암도가 최소 120에서 160까지 일반적인 공기 액체 층의 명암도보다 높게 나타나고 수평 픽셀 수가 적은 경우에는 명암도 값이 높게 나타나고 수평 픽셀 수가 중간인 경우에는 명암도가 낮게 나타난다. 그리고 수평 픽셀 수가 많은 경우에는 명암도 값이 높게 나타나는 공기 액체 층의 일반적인 형태를 가진다.
노란색 라인의 경우에는 명암도가 최소 120에서 140까지 일반적인 공기 액체 층의 명암도와 비슷하게 나타난다. 그리고 수평 픽셀 수가 적은 경우와 많은 경우에는 명암도가 높게 나타나고 수평 픽셀수가 중간인 경우에는 명암도가 낮은 공기 액체 층의 일반적인 형태이지만 명암도가 변화가 적게 나타난다. 
초록색 라인은 수평 픽셀 수에 비례해서 명암도의 변화가 거의 없어서  일반적인 공기 액체 층의 형태와는 다소 차이가  있지만 전체 명암도가 최소 80에서 120까지 일반적인 공기 액체 층의 명암도와 비슷하게 나타난다. 회색 라인은 전체 명암도가 최소 70에서 160까지 명암도가 분포되어 있고 수평 픽셀수가 작은 경우와 많은 경우에 명암도가 높고 수평 픽셀수가 중간일 경우엔 명암도가 낮은 일반적인 공기 액체 층의 형태이다. 
하늘색 라인은 전체 명암도가 최소 70에서 140까지 명암도가 분포되어 있고 수평 픽셀 수가 적은  경우와 많은 경우에 명암도가 높고 수평 픽셀수가 중간일 경우엔 명암도가 낮은 공기 액체 층의 일반적인 형태를 가지는 일반적인 공기 액체 층의 형태이다. 
주황색라인의 경우는 수평 픽셀의 수가 57에서 77까지이고 공기 액체 층의 검은색 부분이 많이 돌출되었기 때문에 나타나는 형태이다. 그러나 주황색 라인은 공기 액체 층의 일반적인 형태인 시작 영역의 몀암도와 끝 영역의 명암도가 높게 나타나고 중간 영역의 명암도는 낮게 나타나는 공기 액체증에 해당된다. 
짙은 파란색 라인은 수평 픽셀수가 많아질 때 명암도가 높아지지 않고 낮아지는 일반적인 공기 액체 층과 구분된 형태를 가지는 예외적인 공기 액체 층에 해당되는 경우이다. 
필터를 만들기 위해서 다수의 장폐색 X-Ray 영상에서 공기액체층 영역의 명암도를 다음과 같은 단계로 공기 액체층 영역을 추출한다.

Step 1. 공기액체층이 표시된 장폐색 영상을 탐색한다.
그림 2(b)와 같은 공기액체층이 빨갛게 표시된 영상데이터를 왼쪽위부터 오른쪽아래까지 픽셀단위로 칼라를 탐색한다.

Step 2. 빨간색 픽셀을 찾으면 X와 Y축의 최대값과 최소값을 기록한다.
그림 2(b)와 같이 빨간색으로 표시된 영역(공기 액체 층 영역)의 좌표를 기록하기 위해 빨간색 픽셀을 찾았을 때 사각형 좌표를 지정하고 사격형 좌표의 시작점(왼쪽 위)인 X와Y좌표의 최소값과 최대값을 기록하고 좌표의 끝점(오른쪽 아래)인 X의 최대값과 Y의 최소값을 기록한다.

Step 3. 모든 영역을 탐색하면서 해당 영상의 좌표를 기록하여 공기액체층 영상 데이터를 따로 추출한다.

Step 4. 모든 영상에 적용시킨다.

Step 5. 추출된 공기 액체층의 영상 데이터 명암도를 가로와 세로의 길이의 평균으로 정규화하여 하나의 필터로 만든다.

추출된 공기 액체층의 각 픽셀의 평균을 구하여 필터를 구성하고 실제 X-Ray 영상에 적용하기 위해 슬라이딩 윈도우 방식으로 원 영상에 식(5)와 같이 Filtering을 한다.

![image](https://user-images.githubusercontent.com/56337609/174418337-20677d03-7d39-4635-9b89-9b529c4423a2.png)
(5)

식(5)에서 는 공기 액체 층의 평균 명암도 값이다. 원 X-Ray 영상에서 슬라이딩 윈도우 방식으로 Feature를 추출하고 각 픽셀별로 ()와 에 대한 편차를 구한다. 그리고 명암도 최대값인 255에서 편차의 차이를 이용하여 해당 픽셀의 feature 값을 구한 후, 최종적인 Feature map을 구성한다. 그림 4는 Feature map을 시각화한 것이다.

![image](https://user-images.githubusercontent.com/56337609/174418368-540d72af-f030-4cab-8ff6-f8143d882a27.png)

### 2.2. Sobel Mask
공기액체층은 중력 때문에 항상 수평을 이룬다는 형태학적 특징을 가지고 있다. 이러한 특징을 검출하기 위해 공기액체층 필터로 생성된 Feature Map에 소벨 마스크[3]을 적용한다.

![image](https://user-images.githubusercontent.com/56337609/174418395-84396046-afe8-4e62-97d7-b83c28154249.png)

소벨 마스크는 영상에서 윤곽선을 검출하는 마스크로 값에 대한 1차 미분값을 사용한다. 소벨 마스크는 윤곽선을 추출할 때 수직 마스크와 수평 마스크를 모두 적용하지만 공기 액체 층의 형태학적 특징을 검출하기 위해 식 (7)과 같이 수평 마스크만 적용한다.

![image](https://user-images.githubusercontent.com/56337609/174418402-5679920d-d0fa-4c4c-9d92-7c4507fb7d3c.png)
(7)

식 (7)에서 수평 마스크는 픽셀들이 있을 때 y축에서 인접한 픽셀과의 1차 미분은 인접 픽셀 값과의 차이를 이용한다.

### 2.3. Histogram Stretching
히스토그램 스트레칭[4]은 비슷한 수치에 몰려 있는 그레이스케일의 값을 확장시키는 과정이지만 일정한 크기로 값을 정규화 한다는 점에서 수치가 넓게 퍼져 있는 Feature Map에서도 적용할 수 있다.
최종적으로 만들어진 Feature Map에 Histogram Stretching을 적용하여 그레이 스케일의 값(0~255)으로 변환하고 정규화 및 시각화를 할 수 있다. 식(8)을 이용하여 히스토그램 스트레칭을 적용한다.

![image](https://user-images.githubusercontent.com/56337609/174418460-b393e5d8-5752-4925-bd29-c5a5e39d1a77.png)
(8)

식 (8)에서 은 Feature값에서 명암도 값으로 변환한 값이고, Feature Map에서 해당 픽셀의 값과 Feature값이 가장 낮은 수를 뺀 값()과 Feature값이 가장 높은 ()수와 가장 낮은 수()를 뺀 값을 나눈 후에 255를 곱하여 각 픽셀의 Feature값을 0~255의 값으로 변환한다. 이 값을 시각화 하여 그림 6(b)과 같이 Activation Map을 생성한다. 

![image](https://user-images.githubusercontent.com/56337609/174418476-f5e25bce-bde2-400c-b31f-37c825a3e4fe.png)

## III. Feature Map with Haar-like feature
앞선 공기 액체 층을 이용한 방법은 형태학적 특징을 이용했기 때문에 공기 액체 층 그 자체를 찾는 데는 도움이 되지만, 형태에 한정되어 명암도가 평균에서 많이 떨어지거나 수평이 기울어지면 장폐색 영역이 추출되지 않는다. 따라서 이러한 문제점을 개선하기 위해서 제안된 방법에서는 원본영상에 Haar-like feature[5]을 적용하여 새로운 Feature Map을 생성한다.
X-Ray 영상에서 공기 액체 층은 위쪽은 공기로 차있기 때문에 검은색에 가깝고 아래는 흰색에 가까운 특징이 있다. 이러한 특징을 Haar-like feature[5]를 적용하여 장폐색 영역을 추출할 수 있다. 그리고 Haar-like feature는 잡음에 둔감하기 때문에 앞의 필터에서 수평에 민감한 특징들을 보완할 수 있다.

### 3.1. 히스토그램 평활화
장폐색의 형태는 공기 액체 층을 기준으로 아래가 밝고 위가 어두운 특징이 있지만 모양이 다양하고 명암에 따라 확실히 구분되는 장폐색 영역이 있는 반면에 명암의 정도가 옅어서 확실히 구분되지 않는 장폐색 영역도 있다. 따라서 히스토그램 평활화로 명암도를 재분배한다.
히스토그램 평활화는  다음과 같이 4단계로 처리된다.

Step 1. 히스토그램을 생성한다.
![image](https://user-images.githubusercontent.com/56337609/174418499-840a09f2-6825-4fcf-a080-ccd30fd705a7.png)

여기서 hist는 영상에서 명암도를 의미하고 영상 픽셀 각각의 명암도(i)를 명암도 1씩 더하여 명암도의 빈도인 히스토그램을 생성한다.

Step 2. 각 픽셀의 i번째 명암도(hist)에 대해 식(9)과 같이 0~i까지의 빈도수의 누적 값(sum)을 구한다.

![image](https://user-images.githubusercontent.com/56337609/174418520-5867c13d-9212-4850-a822-6e44ce9a6694.png)
(9)

Step 3. 구한 명암도 누적 값(sum)을 이용하여 식(10)과 같이 정규화(n)한다. 

![image](https://user-images.githubusercontent.com/56337609/174418533-6c1c8a57-7cfb-48b2-94a8-80ab244a4480.png)
(10)

여기서 N은 영상의 전체 픽셀수이다. 명암도값으로 정규화하기 위해 255을 곱한다.

Step 4. 입력 영상에서 픽셀값 i를 정규화된 n[i]로 변환하여 결과 영상을 생성한다.

평활화는 히스토그램 스트레칭에 비해서 명암 대비를 명확하게 구분한다. 스트레칭과 평활화를 비교한 그림은 그림 7과 같다.

![image](https://user-images.githubusercontent.com/56337609/174418550-b596b74d-d389-44fb-8662-d492c57cc5e8.png)

### 3.2. Haar-like feature
Haar-like feature는 영상에 흑백의 사각형을 겹쳐 밝은 영역에 속한 픽셀 값의 평균과 어두운 영역에 속한 픽셀값들 간의 차이에 대한 평균을 구하는 방법이다. 원래 Haar-like feature는 여러 가지 약 검출기를 윈도우에 결합하여 예측 혹은 분류를 하는 알고리즘이지만 제안된 방법에서는 장폐색의 경우에 공기 액체 층을 제외한 형태가 일정하지 않기 때문에 윈도우에 포함되는 여러 약 검출기를 적용할 수 없다. 따라서 공기 액체 층을 기준으로 위쪽은 상대적으로 어두운 경향과 아래쪽은 밝은 경향에 유효한 feature인 Two-rectangle feature만 적용한다. 공기 액체 층의 형상에 Haar-like feature인 Two-rectangle feature를 겹쳐 차이를 구한 값을 feature로 만든다. Two-rectangle feature를 시각화 한 모습은 그림 8과 같다.

![image](https://user-images.githubusercontent.com/56337609/174418558-25601673-f744-444f-aa3e-fc87689d3843.png)

그림 8과 같은 사각 Filter를 생성하여 입력된 영상에 식(11)과 같이 조건에 따라 Filtering하여 Feature값을 구하고 Feature map을 생성한다.

![image](https://user-images.githubusercontent.com/56337609/174418561-b0b9b1f9-4136-42dc-97fa-915249f0bb59.png)
(11)

사각 윈도우의 픽셀별로 Filtering을 한다. 255(흰색)일 경우에는 입력 영상의 명암도()에서 255를 나눠서 비율을 구한다. 0(검은색)인 경우에는 동일한 기준을 적용하기 위해 255에서 입력 영상의 명암도(x)값을 빼는 연산을 적용하여 feature값을 구한다. 구해진 haar-like feature는 0~1사이의 값을 가지는데, 2장에서 생성된 Activation Map과 같이 적용하기 위해서 히스토그램 스트레칭을 Feature Map에 적용한다. 적용된 결과는 그림 9와 같다.

![image](https://user-images.githubusercontent.com/56337609/174418567-2d0e3934-a110-4c83-a32a-e9f07168bdeb.png)

## IV. 제안된 장폐색 영역 검출 방법
Filter 방법은 장폐색의 형태학적 특징인 공기액체층의 픽셀 빈도수에 대한 가중치를 구하여 적용한다. 따라서 이 방법은 장폐색 추출에 적용할 경우에는 비교적 처리 속도가 다른 방법에 비해 빠르고 장폐색의 형태학적 특징 중에서 수평 형태를 가진 장폐색 영역을 추출하는 데 효과적이다. 그러나 척추, 골반, 횡경막과 같은 부위를 장폐색으로 추출하는 문제점이 발생하고 장폐색 영역의 명암도와 그 외 영역의 명암도와 차이가 적으면 장폐색 영역을 추출할 수 없다.
Haar 방법은 영상에 적용하는 윈도우 영역에서 명암도 차이를 구한다. 따라서 장폐색 특징인 공기 액체층의 명암도는 윗 부분은 검은색에 가까운 명암도를 가지고 아래 부분은 흰색에 가까운 명암도를 가지므로 이러한 특성을 갖는 장폐색 영역을 추출하는데 효과적이다. 그리고  Haar 방법에 적용되는 윈도우의 명암 차이를 이용하기 때문에 잡음에 영향을 적게 받는다. 또한 히스토그램 평활화로 명암도를  재분배하기 때문에 명암도가 뚜렷하지 않은 장폐색에 영역도 추출할 수 있다. 그러나 폐와 같은 영역도 명암도를 재분배하기 때문에 장폐색 영역으로 추출되지 문제점이 있고 처리 연산량이 많아서 시간이 다른 방법들에 비교 많이 소요된다.
따라서 제안된 방법에서는 공기액체층 필터의 Feature map과 Haar-like feature의 Feature Map에 가중치를 각각 적용하여 종합적인 장폐색 영역을 추출한다. 또한 가중치의 적합한 비율을 구하기 위하여 정확도에 대해서 통계 모델을 적용한다.

### 4.1. Smoothing
앞선 각 방법들은 픽셀단위로 장폐색 영역을 추출하기 때문에 장폐색 영역을 컴퓨터로 구분했을 때 장폐색이 아닌 영역이 장폐색 영역으로 추출되는 경우가 많이 발생한다. 따라서 Smoothing으로 Feature map의 디테일을 제거해야한다. Feature map에 Smoothing을 적용하는 필터 마스크는 다음과 같다.

![image](https://user-images.githubusercontent.com/56337609/174418578-d72a6ad0-1d3a-4c26-8824-30bfd60813db.png)

그림 10의 가우시안 필터를 Feature map에 convolution 연산을 적용하여 인접한 픽셀과의 차이를 이용하여 Feature map을 재구성한다.

### 4.2. 장페색 추출 정확도에 대한 통계적 분석

추출된 장폐색 영역이 전문의가 제시한 장폐색 영역과 비교하여 정확하게 추출하였는지 확인하기 위해선 수평위치와 수직위치에 겹쳐있는지를 확인하다. 따라서 블롭 라벨링 기법[6]을 사용하여 추출한 데이터와 전문의가 제시한 데이터간의 비교를 통하여 정확도를 구하는 방법은 다음과 같다.

Step 1. 영상에서 장폐색으로 추출한 영역과 그 외의 영역으로 이진화한다.

Step 2. 이진화된 영상을 탐색하여 인접한 픽셀에 모두 같은 번호(Label)를 붙이고 연결되지 않은 다른 성분에는 다른 번호를 붙여서 객체화한다.

Step 3. 추출된 장폐색 영역과 전문의가 제시한 장폐색 영역에 대해 Blob labeling으로 좌표를 비교하여 장패색 영역의 True Positive(TP), False Positive(FP), False Negative(FN)으로 분류하여 수치를 기반으로 Accuracy(정확도)를 식(12)와 같이 계산한다.

![image](https://user-images.githubusercontent.com/56337609/174418611-c1dfd2cf-205f-4110-baef-f1cced090604.png)
(12)

식 (12)에서 TP(True Positive)는 장폐색 영역을 장폐색 영역으로 추출한 수치이다. FP(False Positive)는 장폐색이 아닌 영역이 장폐색 영역으로 추출된 수치다. FN(False Negative)은 장폐색 영역이 장폐색이 아닌 영역으로 추출된 수치이다.
수치를 모두 더한 값에 TP를 나눔으로써 정확도(Accuracy)를 구한다.

Step4. 장폐색 영역을 추출한 데이터에 공기 액체층 필터 기반 Feature map과 Haar-like feature기반 Feature map의 가중치를 각각 지정하여 정확도의 합계를 식(13)과 같이 구한다.

![image](https://user-images.githubusercontent.com/56337609/174418615-852fea88-6631-4846-8116-410234257df6.png)
(13)

A는 공기액체층 필터기반 Feature map을 의미하고 B는 Haar-like feature기반 Feature map을 의미한다. k는 가중치이며 A에 적용한다. B에는 A에서 적용되는 가중치와 대비될 수 있도록 1에서 뺀 값을 곱해 n개의 모든 영상 데이터에서 i번째에 적용하여 정확도를 모두 구한다. 그리고 가중치(0.1~0.9)에 대한 정확도를 모두 더해 나온 결과(sum)를 구한다. 
제안된 방법에서는 정확도에 대한 통계를 구하기 위해 실험 영상 119장의 장폐색 X-ray영상을 대상으로 실험하였고 결과는 그림 11과 같다.

![image](https://user-images.githubusercontent.com/56337609/174418624-6f28a7f9-6801-4cdb-92ec-d039d648b681.png)

가장 오른쪽의 막대는 A(공기액체층 필터기반 Feature map)이 가중치 0.9로 적용한 결과이고 왼쪽은 B(Haar-like feature기반 Feature map)이 0.9로 적용된 결과이다. 그림 11과 같이 Filter Feature map과 haar Feature map의 가중치를 각각 0.4, 0.6씩 부여하여 원본 영상에 오버랩한 결과는 그림 12와 같다.

![image](https://user-images.githubusercontent.com/56337609/174418632-f10a0b45-4578-428e-b67f-3345035416e7.png)

또한 제안된 방법의 특징에는 부합하지만 장폐색 이외의 영역까지 검출되는 횡경막 위쪽 영역은 히스토그램 분석을 통하여 그림 13과 같이 장폐색 이외의 영역을 제거한다.

![image](https://user-images.githubusercontent.com/56337609/174418639-9babeaf2-77d7-4719-9224-f55d259b94ce.png)

히스토그램을 분석하여 그림 13과 같이 위쪽 검은색 영역의 명암도가 급격히 줄어드는 특징 있는 영역은 임계값을 식 (12)와 같이 설정하여 횡경막 영역 위에 있는 영역을 추출에서 제외한다.

![image](https://user-images.githubusercontent.com/56337609/174418643-25d49ef6-de06-450e-9e4a-3875113502ca.png)
(12)

식 (12)에서 n은 데이터셋의 개수이고 그림 11과 같이 횡경막의 높이 값을 y로 지정하고 평균(Avg)을 구하여 임계값으로 설정한다. 그림 13과 같이 이진화 영상에서 임계값보다 적으면 횡경막 위쪽 영역을 제거하여 각 방법에 적용한다.

## V. 실험 및 결과 분석
제안된 방법에서는 제안된 방법의 성능을 분석하기 위하여 Intel(R) Core(TM) i3-9100F CPU @3.60Hz와 4GB RAM이 장착된 PC상에서 Visual Studio 2017 C#으로 제안된 방법을 구현하고 실험 영상은 119장의 장폐색 X-ray 영상을 대상으로 실험하였다. 장폐색 영역을 추출한 결과는 그림 14와 같다.

![image](https://user-images.githubusercontent.com/56337609/174418671-58af08fc-bdc5-465b-b6ef-7623dc1f9184.png)

제안된 방법에서는 장 폐색 영역의 추출 성능을 분석하기 위하여 Confusion Matrix를 사용하였다. 성능평가는 식 (13)과 같은 Accuracy(정확도)와 Precision(정밀도)과 Recall(재현도)을 적용하였고 결과는 표 2와 같다.

![image](https://user-images.githubusercontent.com/56337609/174418679-c5d484ed-720c-4c66-bc86-f3dc476312c0.png)

식 (13)에서 TP(True Positive)는 장폐색 영역을 장폐색 영역으로 추출한 수치이다. TN(True Negative)은 장폐색이 아닌 영역이 장폐색이 아닌 영역으로 추출된 수치이다. FP(False Positive)는 장폐색이 아닌 영역이 장폐색 영역으로 추출된 수치이다. FN(False Negative)은 장폐색 영역이 장폐색이 아닌 영역으로 추출된 수치이다.

![image](https://user-images.githubusercontent.com/56337609/174418698-8b82d4e4-61d2-489f-9b36-8f93850ac8af.png)

표 2에서와 같이 Filter를 생성하는 방법과 Haar-like feature를 이용한 방법을 각각 적용할 경우보다는 제안된 방법을 적용하여 검출한 경우가 장폐색 추출에 효율적인 것을 표 2에서 확인할 수 있다.

![image](https://user-images.githubusercontent.com/56337609/174418714-a8e68667-96a4-4c65-8f09-5f26329cbefb.png)

그림 15은 Filter  방법과 Haar 방법을 각각 적용하여 장폐색 영역을 추출한 결과와  제안된 방법을 적용하여 장폐색 영역을 검출한 결과를 나타내었다. 그림 15에서 알 수 있듯이 모든 방법에서 장폐색 영역이 추출 되었다.

![image](https://user-images.githubusercontent.com/56337609/174418722-38257b76-152c-4b74-8d41-b24dc46e09df.png)

그림 14는 Filter를 적용한 방법은 장페색 영역의 추출에 실패한 경우와 Haar 방법과 제안된 방법은 장폐색 영역의 추출에 성공한 경우이다. 공기 액체 층을 Filter로 생성한 방법은  수평 형태에 민감하게 반응하여  척추 영역을 장폐색 영역으로 검출하였다. 그러나 Haar 방법은 윈도우의 명암 차이를 이용하기 때문에 수평 형태에 영향을 적게 받고 히스토그램 평활화로 명암도를  재분배하기 때문에 공기 액체 층의 특징을 이용할 수 있기 때문에 장폐색 영역이 추출되었다. 그리고 제안된 방법도 Haar-like feature로 구성한 Feature Map을 적용하기 때문에 장폐색 영역이 주출되었다.

![image](https://user-images.githubusercontent.com/56337609/174418732-dd544e7a-31dd-4de6-8c88-8c8aa369306b.png)

그림 16은 Haar-like feature를 이용한 방법 은 장폐색 영역의 추출에 실패한 경우이고 Filter 방법과 제안된 방법은 장폐색 영역의 추출에 성공한 경우를 나타내었다. Haar-like feature은 평활화를 적용하여 명암도를 재분배하는 과정에서 흉부 영역의 명암도가 장페색 영역의 명암도와 유사하여 흉부 영역이 추출된 경우이다.
 그러나 Filter를 적용한 방법과 제안된 방법은 장폐색의 형태학적 특징인 공기액체층의 픽셀 빈도수에 대한 가중치를 구하여 적용하므로 비교적 정확히 장폐색 영역이 추출되었다.
 
 ![image](https://user-images.githubusercontent.com/56337609/174418738-7a3b03a7-53ba-447b-882f-c2894e7c315c.png)

그림 17은 Filter를 적용한 방법과 Haar 방법 각각 장폐색 영역의 추출에 실패한 경우이지만  제안된 방법은 장폐색 영역의 추출에 성공한 경우이다.  그림 17(a)와 같이 공기 액체 층 Filter를 정의하여 검출한 결과는 수평에 더 많은 영향을 받아 척추 영역을 장페색 영역으로 추출한 경우이고 그림 17(b)은 Haar-like feature를 이용하여 장폐색 영역을 검출한 방법은 명암도에 영향을 상대적으로 더 많이 받기 때문에 장폐색 영역에서 일부 영역을 검출하지 못하는 경우이다. 반면에 제안된 방법은  각 방법이 상호 작용되어 장폐색 영역의 특징이 추출되어서 정상적으로 장폐색 영역이 검출되었다.

![image](https://user-images.githubusercontent.com/56337609/174418749-e9a62f7f-fde5-429a-bcac-f77832ed9119.png)

그림 18은 모든 방법에서 장폐색의 일부만  추출된 경우이다. Filter 방법은 공기 액체층에 수평 마스크를 적용하는 과정에서 수평 에지가 검출되지 않은 경우에는 장폐색 영역을 척추와 골반 영역으로 분류되어 장폐색 영역을 추출에서 제외되는 경우가 발생하였다. Haar 방법은 명암도를 재분배하는 과정에서 장폐색 영역의 명암도가 횡경막의 명암도와 유사하여 장폐색 영역의 추출에서 일부 특징 영역들이 제외되었다. 제안된 방법은 장폐색의 형태학적 특징인 공기 액체층의 픽셀 빈도수에 대한 가중치를 구하는 과정에서 공기액체층의 특징 정보가 손실되는 경우가 발생하였다, 그리고 윈도우 영역에서 명암도 차이를 구한 후에  명암도를 재분배하는 과정에서 황경막의 명암도와 차이가 없는 경우가 발생하여 장페색 영역 중에서 일부만 추출되었다.  

## IV. 결 론
제안된 방법에서는 X-Ray 영상으로 장폐색의 공기액체층을 검출하기 위해 장폐색을 가진 환자의 공기액체층을 Feature Detector로 생성한 후에 Convolution을 적용하고 수평 마스크로 Feature Map을 생성하는 방법과 Haar-like feature로 추가적인 Feature Map을 생성하는 방법을 적용하여 장페색 영역을 추출하는 방법을 제안하였다.
 제안된 방법의 추출 성능을 분석하기 위하여 119장의 장폐색 영상을 대상으로 정확도와 정밀도 및 재현도를 분석하였다.  Filter 방법과 haar를 각각 적용한 방법보다 이 두 방법을 융합한 방법을 적용할 경우가 장폐색 영역 추출에 대한 정확도와 정밀도 및 재현도가 크게 향상된 것을 확인할 수 있었다. 그러나  정확도와 정밀도 및 재현도를 90% 이상 개선하기 위해서는 정밀한 디텍터에 대한 연구가 필요하다.
제안된 융합 방법에서는 필터보다 수평 마스크에 영향을 더 많이 받고, 명암도로 기반으로 추출하기 때문에 척추나 횡경막, 골반 등의 명암도와 장페색 영역의 명암도가 유사한 경우가 발생하여 장페색 영역이 검출되지 않거나 일부만 검출되는 경우가 발생하였다. 
따라서 향후 연구 과제는 이러한 문제점을  개선하기 위한 방법으로 필터를 딥 기반 계층적 클러스터링 기법을 적용하여 세분화하는 방법이나 장페색의 다양한 특징들을 분류할 수 있도록 하기 위해  Deep 기반 Fuzzy Supervised Learning 기법을 연구하여 공간적으로 특징들을 분류할 수 있도록 할 것이다.

## Reference
[1] K. Yoh, et al., “Gallstone ileus: review of 112 patients in the Japanese literature,” The American Journal of Surgery, Vol.140, No.3 pp.437-440. 1980.

[2] H. W. Kim, H. I. Lee, S. I. Park, and K. B. Kim, “Ileus Detection by Using ART2 and Hough Transform”, Korea Institute of Information and Communication Engineering, Vol.21, No.2, pp.488-490, 2017.

[3] Rafael C. Gonzalez and Rafael C. Gonzalez, “Digital Image Processing”, Addison-Wesley Publishing Company, Inc., 1993.

[4] Alan Watt and Fabio Policarpo, "The Computer Image" ADDISON-WESLEY, pp. 120-1, 1998.

[5] P. Viola and M. Jones, “Robust real-time face detection,” in Proc. 8th IEEE Int. Conf. Comput. Vision, vol. 2, pp. 747, Vancouver, Canada, July 2001.

[6] M. Dian. Bah, A. Hafiane, R.l Canals, “Deep Learning with Unsupervised Data Labeling for Weed Detection in Line Crops in UAV Images”, Remote Sensing, Vol.10, Issue 11,  pp.1-22, October 2018.l; doi:10.3390/rs10111690.
