### Ileus Extraction in X-Ray image using Air-fluid Level Filter and Haar-like Feature
# 공기 액체층 필터와 Haar-like feature를 이용한 X-Ray 영상에서의 장폐색 검출

## 요약
장폐색은 소화관의 일부가 완전히 막혀 음식물이 통과하는데 장애를 일으키는 질환이다. 장 폐색 진단은 X-Ray 검사로 가능하지만 육안으로 진단하는 방법은 검사자의 주관이 관여되어 진단 결과에 영향을 끼칠 수도 있기 때문에 장폐색에 대해 명확하고 객관적인 분석이 필요하다. 기존의 연구 방법에서는 소장 및 대장의 장폐색이 대략적이고 일부 영역만이 검출되는 문제점이 있다. 제안된 방법에서는 소화관에서 장폐색을 검출하기 위하여 장폐색의 중요한 특징인 공기액체층(Air-fluid level)영역을 필터로 정의하여 연속적인 Convolution 연산으로 구성한 Feature Map과 Haar-like feature로 구성한 Feature Map을 적용하여 종합적인 분석을 할 수 있는 Activation Map을 생성하고 모든 소화관 영역에 적용하여 픽셀 단위로 장폐색 영역을 추출한다. 제안된 방법을 119장의 장폐색 X-Ray 영상을 대상으로 실험한 결과, 정확도는 71.9%이고 정밀도는 81.4% 및 재현도는 84.8%으로 나타났다. 

## I. 서 론
장폐색은 식도에서 위, 십이지장, 소장, 대장에 이르는 소화관의 일부가 완전히 막혀 음식물이나 소화액이 통과하는데 장애를 일으키는 질환이나 증상을 말한다. 장폐색의 진단은 단순 복부 X-ray 검사로 가능하며, 장폐색이 발병한 환자의 X-ray영상을 관찰하면 장 내부의 물질이 배출되지 못하고 장이 팽창한다. 이 때, 장 내부로 액체와 공기가 모이고 공기는 위쪽으로, 액체는 아래쪽으로 중력에 의해 이동해 공기와 액체가 이루는 면이 자연스럽게 평평하게 되면서 공기 액체 층(Air-fluid level)이 관찰된다.[1] 이러한 특징은 영상학적 검사에서 중요한 진단의 키로서 작용할 수 있다. 검사자는 이러한 특징을 이용해 장폐색을 진단하는데, 육안으로 진단하는 방법은 검사자의 주관이 관여되기 때문에 진단결과에 영향을 미친다. 
기존의 방법[2]은 소장 폐색 영역에서 공기 액체 층이 뚜렷하게 나타나지 않는 X-ray 영상에서는 추출되지 않는 문제점이 있거나 대략적인 소장 및 대장의 장 폐색만 검출하여 정밀하게 분석할 수 없었다. 따라서 본 논문에서는 장폐색의 명암도가 급격히 변화한다는 특징과 공기 액체 층이 항상 수평을 이룬다는 두 가지의 형태학적 특징을 기반으로 대장 및 소장의 폐색 영역을 추출하는 방법을 제시한다. 제안된 방법에서는 공기액체층(Air-fluid level) 영역을 필터로 정의하여 연속적인 Convolution 연산으로 구성한 Feature Map과 Haar-like feature로 구성한 Feature Map에서 각각의 Feature map을 생성한다. Feature map은 haar-like feature의 Filtering으로 각각의 Feature map을 구하고 구한 두 개의 Feature map을 이용하여 Activation Map을 생성하여 픽셀 단위로 공기 액체 층의 특징을 분석하여 장폐색 영역을 검출한다. Activation map은 Feature Map 행렬에 활성화 함수를 적용한 결과이다. 제안된 장 폐색 검출 순서도는 그림 1과 같다.

![image](https://user-images.githubusercontent.com/56337609/174417770-05715c69-cc4a-46b1-a3bc-4f91e362743a.png)

## II. Feature Map with Air-fluid level Filter
공기액체층이 포함된 장폐색 X-Ray 영상은 그림 2와 같다.

![image](https://user-images.githubusercontent.com/56337609/174418233-cb3582c4-28db-42b8-9b03-8246b6c05d12.png)

### 2.1. Feature Detector
