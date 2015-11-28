'use strict'

describe('Guid Service', function () {
    var guidService;
    var guidRegex;

    beforeEach(module('app.shared'));

    beforeEach(function () {
        guidRegex = /^[a-fA-F0-9]{8}(?:-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}$/;
    });

    beforeEach(inject(function (_guidService_) {
        guidService = _guidService_;
    }));

    it('generates a guid', function () {
        var guid = guidService.generate();
        expect(guid).toMatch(guidRegex);
    });

    it('generates a random guid', function () {
        var firstGuid = guidService.generate();
        var secondGuid = guidService.generate();
        expect(firstGuid).toMatch(guidRegex);
        expect(secondGuid).toMatch(guidRegex);
        expect(firstGuid).not.toBe(secondGuid);
    });
});