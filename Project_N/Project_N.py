# 학습용
import numpy as np
import matplotlib.pyplot as plt
import tensorflow as tf
from tensorflow import keras

# 전적 검색용
import requests
from urllib import parse

# 기타
import sys

# 전역변수 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
"""
TOTAL_MATCH_COUNT = 가져올 총 전적의 개수
DEVELOPMENTAPIKEY = 롤 API에 접속하기 위한 개발자키, 하루에 한 번 갱신해야됌
headers = 롤 API에 접속할 때 해당 URL에 넘겨줄 헤더 정보
"""
TOTAL_MATCH_COUNT = 20
DEVELOPMENTAPIKEY = "RGAPI-91f32373-ce49-4269-9fd6-46106f88a22c"
headers = {
        "Origin": "https://developer.riotgames.com",
        "Accept-Charset": "application/x-www-form-urlencoded; charset=UTF-8",
        "X-Riot-Token": DEVELOPMENTAPIKEY,
        "Accept-Language": "ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7",
        "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36"
}

# 함수 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
def getData(reported_user) :
    global headers

    encodingSummonerName = parse.quote(reported_user)
    APIURL = "https://kr.api.riotgames.com/lol/summoner/v4/summoners/by-name/" + encodingSummonerName
    res = requests.get(APIURL, headers=headers)
    data = res.json()

    return data["accountId"] # 신고된 유저의 전적검색용 암호화ID 가져온다

def getAllMatch(encryptaccountId) :
    global TOTAL_MATCH_COUNT
    global headers

    encryptId = encryptaccountId
    APIURL = "https://kr.api.riotgames.com/lol/match/v4/matchlists/by-account/" + encryptId
    res = requests.get(APIURL, headers=headers)
    data = res.json()

    return data['matches'][:TOTAL_MATCH_COUNT] # matches 의 인덱스 = 몇 번째 게임에 대한 정보 출력할지 결정

def getRankedSoloMatch(encryptaccountId) :
    global TOTAL_MATCH_COUNT
    global headers
    rankedsolo_matches = []

    encryptId = encryptaccountId
    APIURL = "https://kr.api.riotgames.com/lol/match/v4/matchlists/by-account/" + encryptId
    res = requests.get(APIURL, headers=headers)
    data = res.json()

    for idx in range(0, len(data['matches'])) :
        if data['matches'][idx]['queue'] == 420 :
            rankedsolo_matches.append(data['matches'][idx])
            if len(rankedsolo_matches) == TOTAL_MATCH_COUNT :
                break

    return rankedsolo_matches

def suspicionMatch(matches, idx, reported_user, abusing_suspicion):
    global headers

    reportedIdx = 0
    abuse_sus = abusing_suspicion
    matchId =  matches[idx]['gameId']
    APIURL = "https://kr.api.riotgames.com/lol/match/v4/matches/" + str(matchId)
    res = requests.get(APIURL, headers=headers)
    data = res.json()

    # 신고 유저의 특정 게임에서 해당되는 인덱스 값을 가져온다.
    for i in range(0, 10) :
        if data["participantIdentities"][i]["player"]["summonerName"] == reported_user :
            reportedIdx = i
            break

    # data["teams"][0] = 블루팀 / data["teams"][1] = 퍼플(레드)팀
    if data["teams"][0]["win"]=="Fail" :
        for i in range(0, 5) :
            # 신고 유저가 아닌 다른 유저일 때만 정보를 저장한다.
            if i != reportedIdx :
                if data["participantIdentities"][i]["player"]["summonerName"] not in abusing_suspicion.keys() :
                    """
                    해당 판의 신고유저가 아닌 다른 유저들의

                    idx = 해당 판이 20판 중 몇 번째 판인지,
                    blue 또는 red = 해당 판에서 무슨 팀이였는지,
                    Win 또는 Fail = 해당 판을 해당 유저는 이겼는지,
                    Friendly 또는 Enemy = 신고유저와 해당유저가 아군이였는지 적군이였는지

                    에 대한 정보를 가져온다.
                    """
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]] = [[idx, "blue", "Fail", (lambda reportedIdx : "Friendly" if reportedIdx<5 else "Enemy")(reportedIdx)]]
                else :
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]].append([idx, "blue", "Fail", (lambda reportedIdx : "Friendly" if reportedIdx<5 else "Enemy")(reportedIdx)])
        for i in range(5, 10) :
            if i != reportedIdx :
                if data["participantIdentities"][i]["player"]["summonerName"] not in abusing_suspicion.keys() :
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]] = [[idx, "red", "Win", (lambda reportedIdx : "Friendly" if reportedIdx>=5 else "Enemy")(reportedIdx)]]
                else :
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]].append([idx, "red", "Win", (lambda reportedIdx : "Friendly" if reportedIdx>=5 else "Enemy")(reportedIdx)])

    if data["teams"][0]["win"]=="Win" :
        for i in range(0, 5) :
            if i != reportedIdx :
                if data["participantIdentities"][i]["player"]["summonerName"] not in abusing_suspicion.keys() :
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]] = [[idx, "blue", "Win", (lambda reportedIdx : "Friendly" if reportedIdx<5 else "Enemy")(reportedIdx)]]
                else :
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]].append([idx, "blue", "Win", (lambda reportedIdx : "Friendly" if reportedIdx<5 else "Enemy")(reportedIdx)])
        for i in range(5, 10) :
            if i != reportedIdx :
                if data["participantIdentities"][i]["player"]["summonerName"] not in abusing_suspicion.keys() :
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]] = [[idx, "red", "Fail", (lambda reportedIdx : "Friendly" if reportedIdx>=5 else "Enemy")(reportedIdx)]]
                else :
                    abuse_sus[data["participantIdentities"][i]["player"]["summonerName"]].append([idx, "red", "Fail", (lambda reportedIdx : "Friendly" if reportedIdx>=5 else "Enemy")(reportedIdx)])

    return abuse_sus

# 메인 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
if __name__ == '__main__' :
    # REPORTED_USER = 신고받은 유저
    REPORTED_USER = '난이쁘고롤도잘함'
    # ABUSING_SUSPICION = 신고 유저와 물주 또는 어뷰저 관계에 있을거라 짐작되는 유저들
    ABUSING_SUSPICION = {}
    # ABUSING_POINT = 신고유저가 어뷰징 유저일 수록 어뷰징 포인트가 높게 측정된다.
    ABUSING_POINT = 0
    # ABUSING_CONFIRM = ABUSING_POINT 가 ABUSING_CONFIRM 을 넘어가면(초과) 신고 당한 유저를 어뷰징 유저로 확정한다.
    ABUSING_CONFIRM = 2

    matches = getRankedSoloMatch(getData(REPORTED_USER)) # TOTAL_MATCH_COUNT 만큼의 전체 또는 솔랭 전적이 담겨있는 List
    for gameIdx in range(0,len(matches)):
        ABUSING_SUSPICION = suspicionMatch(matches, gameIdx, REPORTED_USER, ABUSING_SUSPICION)

    for key in list(ABUSING_SUSPICION.keys()) :
        # 중복유저가 아군인 게임 제외
        remove_Items = []
        for team in ABUSING_SUSPICION[key] :
            if team[3] == "Friendly" : remove_Items.append(team)
        if len(remove_Items) != 0 :
            for item in remove_Items :
                ABUSING_SUSPICION[key].remove(item)

        # 적으로 만난 횟수가 1회인 경우 제외
        if len(ABUSING_SUSPICION[key]) <= 1 :
            del ABUSING_SUSPICION[key]
            
    if not ABUSING_SUSPICION :
        print('\n' + REPORTED_USER + " 는 어뷰징 유저가 아닙니다.")
        sys.exit()
    else :
        print('\n' + REPORTED_USER + " 에 대한 어뷰징 검사에 들어갑니다.")

    # 추출된 전적이 한쪽이 승률 100% 인지 검증하기
    remove_Keys = []
    for key in list(ABUSING_SUSPICION.keys()) :
        win = ""
        for game in ABUSING_SUSPICION[key] :
            if win == "" :
                win = game[2]
            elif win != game[2] :
                remove_Keys.append(key)
                break

    for key in remove_Keys :
        del ABUSING_SUSPICION[key]

    if not ABUSING_SUSPICION :
        print('\n' + REPORTED_USER + " 는 어뷰징 유저가 아닙니다.")
        sys.exit()
    else :
        ABUSING_POINT += 1