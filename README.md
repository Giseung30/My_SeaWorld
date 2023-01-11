# 바다 세계 만들기
> Unity에서 SRP(Scriptable Render Pipeline)인 URP를 공부하면서 만든 프로젝트다. 

## 👍🏻 URP의 장점
+ 정식으로 제작에 사용 가능
+ 한 번만 개발하여 Unity를 지원하는 **모든 플랫폼에 배포 가능**
+ 2D, 3D, XR 등 **모든 종류의 프로젝트 제작에 최적화**
+ 아티스트용 툴(비주얼 이펙트 그래프, **셰이더 그래프**, 렌더 패스) 지원
+ Unity 프로젝트의 **기본 렌더링 옵션**으로 활용 예정
> 기존에 사용하던 빌트인 파이프라인보다 성능도 향상되었다고 하니 안 쓸 이유가 없다..!

## 📣 프로젝트 계기
<p align="center">
  이번 프로젝트는 URP를 적용해보기 위해 만들어진 프로젝트였으니,<br>
  이를 통해 어떤 결과물을 만들어 낼 수 있는지 검색하던 와중 발견한 <b>예시 사진</b>이 마음에 확 꽂혔다.<br>
</p>

<div align="center">
  <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211619842-e797b702-0f7c-49fd-9daa-c37c53a26f9a.png"/>
</div>

> 정말 아름답지 않은가!

<p align="center">
  위 이미지의 바다는 <b>셰이더 그래프</b> 기능으로 구현했다고 한다.<br>
  이 URP 예시 프로젝트를 모방하면서 많이 배우고자 본 프로젝트를 시작하게 되었다.<br>
</p>

```
🔍 셰이더 그래프(Shader Graph)
- 굳이 코드를 작성하지 않고도 노드를 기반으로 시각적으로 셰이더를 저작할 수 있는 기능이다.
- 노드를 시각적으로 생성해 연결하고 프리뷰를 통해 즉시 작업 내용을 확인해 반복 작업 속도를 높일 수 있다.
```

## ⚙ 프로젝트 환경
+ 개발툴 : `Unity 2021.1.0f1`
+ 렌더 파이프라인 : `URP`
+ 모델링 수정 : `Blender`
+ 오디오 수정 : `GoldWave`
+ UI 제작 : `Adobe Photoshop`

## ✏ 프로젝트 기획
+ 기존 바다 배경은 너무 심심하니 집 화장실에서 흔히 볼 수 있는 **욕조**를 배경으로 한다.
+ 물 주변에 **구름**, **갈매기**, **비행기**, **열기구**, **배**가 돌아다닌다.
+ 물속에 **모래**, **바위**, **산호초**를 적절히 배치하여 지형을 구성한다.
+ **울렁이는 효과**, **빛이 들어오는 효과**, **공기 방울 효과** 등을 활용하여 물속 분위기를 구성한다.
+ 사용자가 욕조 주변을 자유롭게 돌아다닐 수 있도록 모듈을 제작한다.

## 📋 개발 내용

### 화장실 배경
> 욕조만 덩그라니 있으면 이상하므로 화장실 배경부터 구성해보았다.

<div align="center">
  <table border="0">
    <tr>
      <td align="center"> 
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211778272-78eca74a-edd7-46cc-8c13-01435047e79a.png"/>
      </td>
      <td align="center"> 
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211778262-3a9eb63a-9bc4-4cd9-b73b-3d663d7a3cbd.png"/>
      </td>
    </tr>
    <tr>
      <td align="center"> 
        Lightmap 적용후
      </td>
      <td align="center">
        Lightmap 적용전
      </td>
    </tr>
  </table>
</div>

<p align="center">
  먼저 <b>에셋 스토어</b>에서 적절한 화장실 가구를 찾아서 배치하고,<br>
  고정되어 움직이지 않는 오브젝트들을 <b>Static</b>으로 지정했다.<br>
  이후, <b>Baked Light</b>를 추가하고, Generate Lighting 과정을 통해 <b>그림자와 음영</b>을 나타냈다.<br>
  각 오브젝트의 반사는 <b>Reflection Probe</b>로 정의했다.
</p>

<div align="center">
  <table border="0">
    <tr>
      <td align="center"> 
        <img width="445em" height="250em" src="https://user-images.githubusercontent.com/60832219/211784538-64415e5a-7a5b-4ee5-bef5-774d849388d2.gif"/>
      </td>
      <td align="center"> 
        <img width="445em" height="250em" src="https://user-images.githubusercontent.com/60832219/211784551-fcda5a2a-ccd8-467e-b001-90f79843de5f.png"/>
      </td>
    </tr>
    <tr>
      <td colspan="2" align="center"> 
        화장실 욕조의 모습
      </td>
    </tr>
  </table>
</div>

<p align="center">
  욕조 위에는 <b>구름, 비행기, 갈매기, 오리</b>가 있다.<br>
  구름은 Paticle System의 <b>Texture Sheet Animation</b>을 활용해서 프레임에 따라 형태가 변화하도록 했다.<br>
  비행기와 오리는 <b>애니메이션</b>을 생성하여 반복적으로 위치를 이동하도록 했다.<br>
  갈매기는 도착지에 도달하면 도착지를 무작위 갱신하는 방식으로 <b>스크립트</b>를 작성했다.<br>
  바다 구현은 추후에 서술한다.
</p>

### 욕조 배경
> 사실, 화장실 배경 씬에서 모든 것을 구현하고 싶었지만, 욕조 크기에 비해 화장실 크기가 너무 커서 메모리 이슈가 발생했기에 욕조 배경 씬을 따로 생성했다. 아래는 욕조 배경의 모습이다.

<div align="center">
  <table border="0">
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793068-92a34014-7c20-4950-a7e6-e34d1a0f7de9.png"/>  
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793082-2689210a-d9cf-4983-8876-f19f8d21d1ae.png"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793096-a66eaa56-0927-46dd-9f63-b0d425ce49a4.png"/>  
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793101-b8d31f08-d493-4a78-9aed-eb165140598c.png"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793073-c0360bfd-8bc1-4443-b0dc-71e54de05896.png"/>  
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793088-b51470fe-968f-4aaf-bfba-fe09d0cedb99.png"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793079-651fef2d-da1c-495c-a734-6d03592e4df3.png"/>  
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211793090-f2e34047-19cd-43b0-832a-936346758d44.png"/>
      </td>
    </tr>
  </table>
</div>

- - -

### 👉🏻 물 셰이더 구현

<div align="center">
  <table>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211807764-9aa47cea-3d16-465d-9ac4-131ecaa19945.gif"/>
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211807783-696562e4-2d54-483f-9fd1-34504553feb9.gif"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        물위
      </td>
      <td align="center">
        물속
      </td>
    </tr>
  </table>
</div>

<p align="center">
  기본적으로 물속 안이 비쳐야 하고, 깊이에 따른 농도를 구현해야 하므로<br>
  <b>Opaque Texture</b>와 <b>Depth Texture</b>가 활성화되어 있어야 한다.
</p>

<div align="center">
  <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211811300-2e8e8077-9d8f-41cd-b97b-8837ef022851.PNG"/>
</div>

<p align="center">
  <b>Refraction</b> 모듈은 물 내부를 보여주고, 물결에 따라 울렁이는 효과를 준다.<br>
  Scene Color 노드에 Noise한 UV를 적용하는 방식이다.<br>
</p>

<div align="center">
  <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211812574-c8e88e36-020f-49f1-ba78-e2aafe65cae7.PNG"/>
</div>

<p align="center">
  <b>Normal</b> 모듈은 두 가지의 Normal Texture를 시간의 흐름에 따라 이동하는 모듈이다.<br>
  빛에 반사되는 물의 표면을 표현한다.<br>
</p>

<div align="center">
  <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211829255-b71bdb16-f33f-44f2-8269-b3d9b7e9ab6b.PNG"/>
</div>

<p align="center">
  <b>Wave</b> 모듈은 시간의 흐름에 따라 물 표면에 파도를 일으키는 모듈이다.<br>
  각 버텍스의 Y축 높이를 Gradient Noise에 맞춰 조절한다.<br>
</p>

- - -

### 👉🏻 Caustic 셰이더 구현
> Caustic이란 정확한 직역은 힘들지만, 빛이 물체에 반사 혹은 굴절되어서 나오는 광이 다른 물체에 맺히는 현상이라고 한다. 물속에서는 이러한 현상이 빈번하므로 현실성을 위해 구현해보고자 했다.

<div align="center">
  <table>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211851226-1c3fb302-84d2-410e-9215-d3df8c37ef02.gif"/>
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211851241-c23d06bb-9f21-4af1-9d70-623241083ef1.gif"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211851239-69cd67da-13f0-4f1e-b17a-8cd0ba92d856.gif"/>
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211851244-31731c88-d4cc-49cf-b1ce-cf8c90a278d5.gif"/>
      </td>
    </tr>
  </table>
</div>

<div align="center">
  <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211852120-b2cbd5c1-410a-4423-94be-e65fa2a6f9fd.PNG"/>
</div>

<p align="center">
  <b>Caustic</b> 셰이더는 간단하게 구현할 수 있었다.<br>
  광이 맺힐 오브젝트의 텍스쳐 노드에 <b>Voronoi</b> 노드가 생성한 Caustic 패턴을 입혀주기만 하면 된다.<br>
  필요시, Caustic 패턴의 색상을 변경할 수 있도록 수정했다.
</p>

- - -

### 👉🏻 Post Processing
> 포스트 프로세싱은 기존에 렌더링된 씬에 렌더링 효과를 더하는 작업이다. 물 밖과 물속의 분위기를 구분하기 위해서 활용됐다.

<div align="center">
  <table>
    <tr>
      <td colspan="2" align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211855678-68138e06-68e5-4b72-b35b-0eafe2ac8679.PNG"/>
      </td>
    </tr>
    <tr>
      <td colspan="2" align="center">
        포스트 프로세싱의 적용 범위
      </td>
    </tr>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211855685-4094113e-dac1-4531-8f1b-5dce54f6cea9.png"/>
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211855690-cc5bede7-aba9-415a-89e0-9cfddcff2811.png"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        포스트 프로세싱 적용 전
      </td>
      <td align="center">
        포스트 프로세싱 적용 후
      </td>
    </tr>
  </table>
</div>

<div align="center">
  <img width="45%" height="45%" src="https://user-images.githubusercontent.com/60832219/211856870-2a7e9729-c7bf-40cc-a97c-742027075c6f.PNG"/>
</div>

<p align="center">
  물속에서만 해당 포스트 프로세싱이 적용되어야 하므로, 적용 범위를 <b>Local</b>로 설정했다.<br>
  <b>Split Toning</b>으로 밝고 푸른 느낌의 색상을 적용하고, <b>Chromatic Aberration</b>으로 번짐 효과를 적용했다.<br>
</p>

- - -

### 👉🏻 공기 방울 구현
> 산호초가 배치되어 있는 곳에만 공기 방울이 생성된다.

<div align="center">
  <img width="75%" height="75%" src="https://user-images.githubusercontent.com/60832219/211859796-bbb16a07-4b4b-458a-814b-aa021ab9207a.gif"/>
</div>

<p align="center">
  Paticle System의 <b>Texture Sheet Animation</b>을 활용해서 프레임에 따라 변하는 입자들을 생성하는 방식으로 구현했다.<br>
  입자들은 생성되는 순간 Y축 방향으로 속도계를 가진다.<br>
</p>

- - -

### 👉🏻 Sun Shaft 구현
> 정의는 '매우 밝은 광원의 일부분 가려질 때 빛이 방사성으로 흩어지는 모습'이다. 쉽게 말해 빛이 물을 뚫고 들어오는 모습이다.

<div align="center">
  <table>
    </tr>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211862622-5d252db0-c789-4ca3-833c-9c07bf766b29.png"/>
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211862633-9075d662-2c28-4282-a115-71088ce61692.png"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        Sun Shaft 적용
      </td>
      <td align="center">
        Sun Shaft 미적용
      </td>
    </tr>
  </table>
</div>

<div align="center">
  <img width="50%" height="50%" src="https://user-images.githubusercontent.com/60832219/211863076-7376bca9-6c5d-4d29-a876-907e10930ccc.PNG"/>
</div>

<p align="center">
  간단히 반투명 머티리얼을 적용해서 배치하는 것으로 구현했다.
</p>

- - -

### 👉🏻 배, 열기구, 풍차
> 열심히 해변 분위기를 만들어주는 고마운 친구들이다.

<div align="center">
  <table>
    </tr>
    <tr>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211865300-23888f7d-e860-40e0-bf96-3f0b325cae9b.gif"/>
      </td>
      <td align="center">
        <img width="100%" height="100%" src="https://user-images.githubusercontent.com/60832219/211865317-db0fafa8-03ba-4108-9c91-312dbad99e27.gif"/>
      </td>
    </tr>
    <tr>
      <td align="center">
        배와 열기구
      </td>
      <td align="center">
        풍차
      </td>
    </tr>
  </table>
</div>

<p align="center">
  각 오브젝트에 <b>애니메이터</b>를 생성하여, 반복 재생했다.
</p>

## ✔ 프로젝트 후기
+ 새로운 기술을 공부하겠다고 프로젝트를 시작했는데, 나름 재밌어서 게임할 시간에도 만들곤 했다.
+ 본래 활용하고자 했던 셰이더 그래프 뿐만 아니라, Particle System도 많이 활용하다보니까 따로 많이 찾아보게 되었다.
+ 중간에 욕심이 생겨서 모바일에서도 구동해볼 수 있는 환경을 구성해보고자 했다.
+ 이 과정을 통해, 폴리곤을 줄이는 방법과 병합 과정으로 Batches를 줄이는 방법을 배웠다.
+ 조이스틱을 구현해서 모바일에서도 돌아다닐 수 있게 했다.
+ 나중에 물고기도 헤엄치게 만들어야겠다.
+ ~~사실 '바다 세계'가 아니라 '욕조 세계'가 맞지 않나~~