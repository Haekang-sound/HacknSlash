
<img width="2559" height="1439" alt="Image" src="https://github.com/user-attachments/assets/95e04f17-fa84-4c63-ad40-3a4a95375059" />

# Unity 프로젝트 : HacknSlash

실행환경 : Winwdows\
게임영상 : [https://youtu.be/avXhfSrf8Zw](https://youtu.be/ZsKCpC8eldA)

## 1. 프로젝트 개요
   111퍼센트 과제 전형 작업물(불합)\
   HacknSlash는 빠른 템포의 전투와 조작감을 강조한 2D 횡스크롤 액션 게임입니다. 플레이어는 이동하며 몰려오는 몬스터 웨이브를 방어하고 생존해야 합니다.
   ### 1.1 조작방법
   | **구분** | **기능** | **조작키 (Key)** |
| --- | --- | --- |
| **이동** | 좌우 이동 | `A`, `D` |
|  | 점프 | `W` |
|  | 앉기 | `S` |
| **회피** | 구르기 / 대쉬 | `S` + `A` 또는 `D` |
| **전투** | 일반 공격 | `L-Click` (마우스 좌클릭) |
|  | 원거리 공격 | `R-Click` (마우스 우클릭) |
|  | 공중 공격 | (공중에서) `L-Click` |

## 2. UnitData
플레이어, 몬스터의 베이스가 되는 클래스, 게임 내부에서 행동하는 오브젝트들을 ‘유닛’으로 규정하며 유닛이 행동할때 필요한 기본젝인 데이터들을 집약해놓은 클래스입니다. 이 클래스를 상속받은 Player와 Monster클래스는 UnitData를 상속받고 필요한 데이터를 추가해서 사용합니다.

## 3. PlayerSMB
PlayerSMB는 애니메이터의 StateMachineBehaviour를 상속받아 Player의 동작에 필요한 함수들을 모아놓은 클래스입니다. 플레이어와 관련된 Behaviour들은(idle, move, attack …) PlayerSMB를 상속받아 필요한 함수들을 사용합니다.

## 4. MonsterManager
몬스터웨이브를 관리하는 몬스터 매니저 입니다. 2타입의 몬스터 웨이브를 관리 할 수 있도록 설계되었습니다.
1. 몬스터설정을 위한 매개변수 : MonsterPrefab A/B, WayPointA/B
2. 웨이브 설정을 위한 매개변수 : PoolSizeA/B, SpawnInterval, WaveSize, WaveDuration, SpawnA/B, WaveTimer, WaveCount
<img width="671" height="865" alt="Image" src="https://github.com/user-attachments/assets/d129e855-979c-48e2-b166-3d387869716c" />

   ### 4.1 Wave
   지정된 웨이브 데이터에 따라 순차적으로 몬스터 생성, GC부하를 막기위해서 몬스터들을 ObjectPool로 관리하며 설정된 값을 통해 Wave를 관리합니다. 
   1. PoolSizeA/B : 각 몬스터를 담아둘 pool의 크기를 설정합니다.
   2. SpawnInterval : 몬스터 생성주기를 설정합니다. (1 == 1초)
   3. WaveSize : 한 웨이브에 등장하는 몬스터 갯수를 설정합니다.
   4. WaveDuration : 웨이브사이의 간격을(시간) 설정합니다.
   5. SpawnA/B : 생성되는 몬스터의 스폰위치를 설정합니다.
   6. WaveTimer : 웨이브가 시작되고 몇초가 지났는지 표시합니다.
   7. WaveCount : 현재 살아있는 몬스터의 갯수를 표시합니다.

   ### 4.2 WayPoint
   몬스터의 이동 경로를 지정하는 웨이포인트 시스템, 몬스터의 이동경로를 설정합니다. 몬스터는 Idle상태일때 설정된 웨이포인트를 순서대로 배회합니다.

   ### 4.3 ObjectPool
   몬스터의 Get/Return과 생성까지 관리하는 오브젝트 Pool입니다. Genelec으로 설계되어 Component를 상속받은 대상을 관리합니다.
   1. 오브젝트 풀생성자 : 생성대상, 초기갯수, 상위 오브젝트를 입력받아 오브젝트풀을 생성하고 parent의 하위 오브젝트로 관리합니다.
   2. AddPool : 오브젝트를 생성하고 Pool에 추가합니다.
   3. CreateNew : 오브젝트를 생성합니다.
   4. Get : 비활성화된 오브젝트를 활성화하고 꺼내옵니다. 대기중인 오브젝트가 없을 경우 생성합니다.
   5. Return : 오브젝트를 비활성화하고 pool로 반환합니다.

## 5. 개선사항
해당 프로젝트는 4일 동안 진행되었으며 인지했으나 해결하지 못한 오류들이 존재했습니다.

   ### 5.1 Singleton 남용
   빠른 과제수행을 위해 매니저와 플레이어 클래스에 Singleton 패턴을 적용했으나, 이는 결합도(Coupling)를 높이고 테스트를 어렵게 만듦. 

   ### 5.2 Animatotr Update 이해부족
   애니메이터 업데이트 주기의 대한 학습을 제대로 하지 못했다. 당연히 프레임단위로 업데이트될 텐데 물리이동을 StateMachineBehaviour에서 진행했고 심지어 플레이어만 임시방편으로 deltaTime을 곱해주고  몬스터에는 적용하지도 않았다.
   
   ### 5.3 C#의 대한 이해부족
   SerialziedField의 제대로 된 용도라던가. 박싱과 언박싱을 알고 있었지만 어떤부분에서 조심했어야 하는지 잘 몰랐다.

   ### 5.4 오작동
   사망후 게임오버가 되었을때 리셋버튼이 제대로 동작하지 않는다.

   ### 5.5 파일정리  
   스크립트 폴더의 정리가 제대로 이루어지지 못했습니다.
